from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date
from app_schedule.models import Subjects_info , User_subjects
from django.db.models import Q
from django.utils import timezone
from datetime import date
import datetime
from django import template
import sqlite3


register = template.Library()

user_sub = []
day_start_times_used = {'M':[],'T':[],'W':[],'H':[],'F':[],'S':[]}
start_times = ['08:00','09:00','10:00','11:00','12:00','13:00','14:00','15:00','16:00','17:00','18:00','19:00']

@register.simple_tag
def get_dict_value(day) :
    return day_start_times_used[day]

# Create your views here.
@login_required(login_url='login')
def selects_subject_view(request):
    error = False
    duration = 1
    sub_date = Subjects_Test_Date.objects.all()
    sub_objects = Subjects_info.objects.all()
    user_id = request.user.id
    user = User_subjects.objects.filter(user_id_id = user_id)


    days = {
    "M": "Monday",
    "T": "Tuesday",
    "W": "Wednesday",
    "H": "Thursday",
    "F": "Friday",
    "S": "Sunday",
}
    
    
    # search btn
    if 'q' in request.GET:
        search = request.GET['q']
        multiple_search = Q(Q(name__icontains=search) | Q(code__icontains=search) | Q(prof__icontains=search))
        sub_name = Subjects_info.objects.filter(multiple_search)
    else:
        sub_name = Subjects_info.objects.none()
    # select_btn 
    if 'select_btn' in request.POST  :
        # The select button was clicked
        subject_id = request.POST.get('id')

        Is_subjects_registed = check_already_regis(subject_id,user_id)
        Is_subjects_passed =  check_pass_subject(subject_id,user_id)
        Is_over_credit =  check_over_credit(user_id,subject_id)

        Is_day_overlapse = check_study_day(subject_id,user_id) 
        a = check_midterm_day(subject_id,user_id) 
        b = check_final_day(subject_id,user_id)
        
        if Is_subjects_registed and Is_subjects_passed and Is_over_credit and Is_day_overlapse and a and b:
            # insert name into user table using Django ORM
            User_subjects.objects.create(user_id_id=user_id, sub_id_id=subject_id)
            durations = Subjects_info.objects.get(pk=subject_id).get_duration()
            start_time = Subjects_info.objects.filter(id=subject_id).first().start_time
            day = Subjects_info.objects.filter(id=subject_id).first().day
            day_start_times_used[day].append(start_time .strftime('%H:%M'))
            user_sub.append(user.last().sub_id.id)

            for duration in range(int(durations)) :
                duration_time = datetime.datetime.combine(datetime.date.today(), start_time) + datetime.timedelta(hours=duration)
                day_start_times_used[day].append(duration_time.time().strftime('%H:%M'))

        else :
            error = True

          

    elif 'delete_btn' in request.POST:
        # The delete button was clicked
        subject_id = request.POST.get('id')
        # Delete subject from user table using Django ORM
        User_subjects.objects.filter(user_id_id=user_id, sub_id_id=subject_id).delete()
        durations = Subjects_info.objects.get(pk=subject_id).get_duration()
        start_time = Subjects_info.objects.filter(id=subject_id).first().start_time
        day = Subjects_info.objects.filter(id=subject_id).first().day
        day_start_times_used[day].remove(start_time .strftime('%H:%M'))
        user_sub.remove(Subjects_info.objects.get(pk=subject_id).id)

        for duration in range(int(durations)) :
           duration_time = datetime.datetime.combine(datetime.date.today(), start_time) + datetime.timedelta(hours=duration)
           day_start_times_used[day].remove(duration_time.time().strftime('%H:%M'))

    context = {'sub_date':sub_date, 
               'sub_name':sub_name ,
               'sub_objects':sub_objects,
               'users':user,
               'start_times':start_times,
               'day_start_times_used':day_start_times_used,
               'user_subj':user_sub,
               'error':error,
               'days':days}
    
    return render(request, 'select_subject.html' , context)

def check_already_regis(sub_id, u_id):
    selects_subjects = User_subjects.objects.filter(user_id_id=u_id).values_list('sub_id_id', flat=True)#วิชาที่select ไปแล้ว

    all_code = []

    for i in selects_subjects:
        all_code_sub = Subjects_info.objects.filter(id=i).values_list('code', flat=True).first()
        all_code.append(all_code_sub)

    code_subject = Subjects_info.objects.filter(id=sub_id).values_list('code', flat=True).first() #เอารหัสวิชาที่userเลือก

    if code_subject in all_code:
        return False
    return True

def check_pass_subject(sub_id,u_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()

    #เลือกวิชาทั้งหมดที่มีจาก subjects ของ user คนนั้น
    c.execute("SELECT real_subject_id FROM subjects WHERE UserID = ?", (u_id,))
    total_subjects = c.fetchall()

    #ไปค้นหารหัสวิชาจาก sub_id ที่ user ป้อนเข้ามา
    c.execute("SELECT code FROM app_schedule_subjects_info WHERE ID = ?", (sub_id,))
    select_sub = c.fetchall()

    # Close the connections
    conn.close()

    if select_sub[0] in total_subjects:
        return False
    else:
        return True
    
 #Function check credit
def check_over_credit(user_id,sub_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()
    # Execute the SELECT statement and retrieve the total sum of credits
    c.execute("SELECT SUM(app_schedule_subjects_info.credit) FROM app_schedule_subjects_info,app_schedule_user_subjects "+
              "WHERE app_schedule_user_subjects.user_id_id = ?  AND app_schedule_user_subjects.sub_id_id = app_schedule_subjects_info.ID ",(user_id,))
    result = c.fetchone()

    # Get the total sum of credits from the result tuple
    if result[0] is not None:
        total_credits = result[0]
    else:
        total_credits = 0
 

    # Execute the SELECT statement to retrieve the sum of credits for the specified subject
    c.execute("SELECT credit FROM app_schedule_subjects_info WHERE ID = ?", (sub_id,))
    result = c.fetchone()
    
    # Get the sum of credits for the specified subject from the result tuple
    
    if result[0] is not None:
        subject_credits = result[0]
    else:
        subject_credits = 0
    subject_credits = result[0]

    # Close the connections
    conn.close()
    
    credits_now = total_credits + subject_credits
    maxcredit = 22
    
    if credits_now > maxcredit :
        return False
    else:
        return True
    
def Check_time_Overlapse(starttime_1,endtime_1,starttime_2,endtime_2):
    hour_starttime_1 = int(starttime_1.split(':')[0])
    min_starttime_1 = int(starttime_1.split(':')[1])

    hour_endtime_1 = int(endtime_1.split(':')[0])
    min_endtime_1 = int(endtime_1.split(':')[1])

    hour_starttime_2 = int(starttime_2.split(':')[0])
    min_starttime_2 = int(starttime_2.split(':')[1])

    hour_endtime_2 = int(endtime_2.split(':')[0])
    min_endtime_2 =  int(endtime_2.split(':')[1])

    

    if hour_endtime_1 <= hour_starttime_2 or  hour_endtime_2 <=  hour_starttime_1:              #เช็คว่ามีชั่วโมงซ้อนกันหรือไม่
        if hour_starttime_1 == hour_starttime_2 :                                                   #เวลาเริ่มเท่ากันน
            if min_starttime_1 == min_starttime_2:                                                      #นาทีเริ่มเท่ากันไม่ได้
                return False
            elif  min_starttime_1 < min_starttime_2:                                                    #นาทีตอนเริ่มอันแรกน้อยกว่าอันสอง หมายความว่าอันแรกเกิดก่อน
                if hour_endtime_1 > hour_starttime_2:                                                       #ชั่วโมงจบของอันแรกก็จะมากกว่าชั่วโมงเริ่มอันที่2ไม่ได้
                    return False
                elif hour_endtime_1 == hour_starttime_2:                                                    #ถ้าชั่วโมงจบของอันแรกเท่ากับชั่วโมงเริ่มอันที่2ต้องมาเช็คนาทีต่อ
                    if min_endtime_1 <= min_starttime_2:                                                        #ถ้านาทีจบของอันแรกเท่าหรือน้อยกว่านาทีเริ่มอันที่2ก็ไม่เป็นไร เพราะเท่ากับว่าอันแรกจบแล้วถึงมาเริ่มอันสอง
                        return True
                    else:
                        return False                                                                            #นาทีมากกว่าไม่ได้เพราไม่งั้นเท่ากับว่ามันจะจบหลังจากอันสองเริ่ม
            elif  min_starttime_1 > min_starttime_2:                                                    #นาทีตอนเริ่มอันแรกมากกว่าอันสอง หมายความว่าอันสองเกิดก่อน
                if hour_endtime_2 > hour_starttime_1:                                                       #ชั่วโมงจบของอันสองมากกว่าชั่วโมงเริ่มอันแรกไม่ได้
                    return False
                elif hour_endtime_2 == hour_starttime_1:                                                    #ถ้าชั่วโมงจบของอันสองเท่ากับชั่วโมงเริ่มอันแรกต้องมาเช็คนาทีต่อ
                    if min_endtime_2 <= min_starttime_1:                                                        #ถ้านาทีจบของอันสองเท่าหรือน้อยกว่าก็ไม่เป็นไร
                        return True
                    else:
                        return False                                                                            #นาทีมากกว่าไม่ได้
        elif hour_starttime_1 == hour_starttime_2 and  hour_endtime_2 ==  hour_endtime_1:           #ทั้งเวลาเริ่มและเวลาจบเท่ากันไม่ได้
            return False       
        else:
            return True                                                                         #ชั่วโมงไม่ได้มีเท่ากันเลยก็ไม่เป็นไร
    else:
        return False                                                                            #ชั่วโมงซ้อนทับกัน
    
def check_study_day(sub_id,user_id):
      conn = sqlite3.connect("w3.db")
      c = conn.cursor()
      c.execute("SELECT sub_id_id FROM app_schedule_user_subjects WHERE user_id_id = ? ",(user_id,)) 
      totalsubject = c.fetchall() #subject ทั้งหมด ที่ user ได้ select ไปแล้ว
      totaltimeofsub = [] #เก็บ day,start time,end time ของแต่ละ subject ที่ user select ไปแล้ว
      if totalsubject != [] and totalsubject[0][0] != None:
            for x in totalsubject:
                  sub = x[0]
                  c.execute("SELECT day,start_time,end_time FROM app_schedule_subjects_info WHERE ID = ? ",(sub,))
                  time = c.fetchall()
                  totaltimeofsub.append(time[0])

      c.execute("SELECT day,start_time,end_time FROM app_schedule_subjects_info WHERE ID = ? ",(sub_id,))
      subject_select = c.fetchall() #เก็บ  day,start time,end time ของวิชาที่ user ต้องการ select ตอนนี้
      if totaltimeofsub != []:
            for y in totaltimeofsub:
                  print(y[0],y[1],y[2])
                  day_subject_selected = y[0]         # day จาก subject ที่ user ได้ select ไปแล้ว
                  starttime_subject_selected = y[1]   # start time จาก subject ที่ user ได้ select ไปแล้ว
                  endtime_subject_selected = y[2]     # end time จาก subject ที่ user ได้ select ไปแล้ว

                  day_subject_select = subject_select[0][0]           # dayจาก subject ที่ user ต้องการ select 
                  starttime_subject_select = subject_select[0][1]     # start time จาก subject ที่ user ต้องการ select 
                  endtime_subject_select = subject_select[0][2]       # end time จาก subject ที่ user ต้องการ select 


                  if day_subject_select == day_subject_selected:
                        if Check_time_Overlapse(starttime_subject_selected,endtime_subject_selected,starttime_subject_select,endtime_subject_select):
                              return True
                        else: 
                              return False
      return True

def check_midterm_day(sub_id,user_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()

    c.execute("SELECT testdate.mid_numday ,testdate.mid_starttime,testdate.mid_endtime "+
              "FROM app_schedule_user_subjects AS user , app_schedule_subjects_info AS info, app_select_subjects_test_date AS testdate " +
              "WHERE user.user_id_id = ?"+
               "AND user.sub_id_id = info.ID " +
               "AND info.code = testdate.code ",(user_id,))  
    totalsubject = c.fetchall()

    c.execute("SELECT testdate.mid_numday ,testdate.mid_starttime,testdate.mid_endtime "+
              "FROM app_schedule_subjects_info AS info, app_select_subjects_test_date AS testdate " +
              "WHERE ?= info.ID " +
               "AND info.code = testdate.code ",(sub_id,))  
    subject_select = c.fetchall()

    if totalsubject != []:
        if  totalsubject[0] != (None,None,None):
            for x  in totalsubject:
                sub_selected = x[0].split("-")
                year,month,day = int(sub_selected[0]),int(sub_selected[1]),int(sub_selected[2])

                day_subject_selected = date(year,month,day)                                     # day จาก subject ที่ user ได้ select ไปแล้ว
                starttime_subject_selected = x[1]                                               # start time จาก subject ที่ user ได้ select ไปแล้ว
                endtime_subject_selected = x[2]                                                 # end time จาก subject ที่ user ได้ select ไปแล้ว

                sub_select = subject_select[0][0].split("-")
                year,month,day = int(sub_select[0]),int(sub_select[1]),int(sub_select[2])

                day_subject_select = date(year,month,day)                                       # dayจาก subject ที่ user ต้องการ select 
                starttime_subject_select = subject_select[0][1]                                 # start time จาก subject ที่ user ต้องการ select 
                endtime_subject_select = subject_select[0][2]                                   # end time จาก subject ที่ user ต้องการ select

                if day_subject_select == day_subject_selected:
                    if Check_time_Overlapse(starttime_subject_selected,endtime_subject_selected,starttime_subject_select,endtime_subject_select):
                        return True
                    else: 
                        return False
    return True

def check_final_day(sub_id,user_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()

    c.execute("SELECT testdate.fin_numday ,testdate.fin_starttime,testdate.fin_endtime "+
              "FROM app_schedule_user_subjects AS user , app_schedule_subjects_info AS info, app_select_subjects_test_date AS testdate " +
              "WHERE user.user_id_id = ?"+
               "AND user.sub_id_id = info.ID " +
               "AND info.code = testdate.code ",(user_id,))  
    totalsubject = c.fetchall()
    print(totalsubject)

    c.execute("SELECT testdate.fin_numday ,testdate.fin_starttime,testdate.fin_endtime "+
              "FROM app_schedule_subjects_info AS info, app_select_subjects_test_date AS testdate " +
              "WHERE ?= info.ID " +
               "AND info.code = testdate.code ",(sub_id,))  
    subject_select = c.fetchall()
    #print(subject_select)


    if totalsubject != []:
        if  totalsubject[0] != (None,None,None):
            for x  in totalsubject:
                sub_selected = x[0].split("-")
                year,month,day = int(sub_selected[0]),int(sub_selected[1]),int(sub_selected[2])
                #print(year,month,day)
                day_subject_selected = date(year,month,day)                                     # day จาก subject ที่ user ได้ select ไปแล้ว
                starttime_subject_selected = x[1]                                               # start time จาก subject ที่ user ได้ select ไปแล้ว
                endtime_subject_selected = x[2]                                                 # end time จาก subject ที่ user ได้ select ไปแล้ว
                #print(day_subject_selected,starttime_subject_selected,endtime_subject_selected)

                sub_select = subject_select[0][0].split("-")
                year,month,day = int(sub_select[0]),int(sub_select[1]),int(sub_select[2])
                day_subject_select = date(year,month,day)                                       # dayจาก subject ที่ user ต้องการ select 
                starttime_subject_select = subject_select[0][1]                                 # start time จาก subject ที่ user ต้องการ select 
                endtime_subject_select = subject_select[0][2]                                   # end time จาก subject ที่ user ต้องการ select
                print(day_subject_select,starttime_subject_select,endtime_subject_select)

                if day_subject_select == day_subject_selected:
                    if Check_time_Overlapse(starttime_subject_selected,endtime_subject_selected,starttime_subject_select,endtime_subject_select):
                        return True
                    else: 
                        return False
    return True 