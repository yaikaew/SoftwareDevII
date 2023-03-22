from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date
from app_schedule.models import Subjects_info , User_subjects
from app_users.models import Subjects
from django.db.models import Q
from django.utils import timezone
from datetime import date
import datetime
from django import template
import sqlite3
from collections import defaultdict

# Initialize dictionary to store user data
user_data = defaultdict(dict)

register = template.Library()

start_times = ['08:00','09:00','10:00','11:00','12:00','13:00','14:00','15:00','16:00','17:00','18:00','19:00']



@register.simple_tag
def get_dict_value(day,user_id) :
    return user_data[user_id]['day_start_times_used'][day]

# Create your views here.
@login_required(login_url='login')
def selects_subject_view(request):
    
    user_id = request.user.id

    if user_id not in user_data:
        user_data[user_id] = {'user_sub': [], 'day_start_times_used': {'M':[],'T':[],'W':[],'H':[],'F':[],'S':[]}}

    Is_subjects_registed = None
    Is_subjects_passed =  None
    Is_over_credit =  None
    Is_day_overlapse = None
    Is_midterm_overlapse = None
    Is_final_overlapse = None

    duration = 1
    sub_date = Subjects_Test_Date.objects.all()
    sub_objects = Subjects_info.objects.all()
    user = User_subjects.objects.filter(user_id_id = user_id)

    user_sub = user_data[user_id]['user_sub']
    day_start_times_used = user_data[user_id]['day_start_times_used']


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
        Is_midterm_overlapse = check_midterm_day(subject_id,user_id) 
        Is_final_overlapse = check_final_day(subject_id,user_id)
        
        if Is_subjects_registed is True and Is_subjects_passed is True and Is_day_overlapse is True and Is_final_overlapse is True and Is_over_credit is True and Is_midterm_overlapse is True :
            # insert name into user table using Django ORM
            User_subjects.objects.create(user_id_id=user_id, sub_id_id=subject_id)
            # duration_time of subject
            durations = Subjects_info.objects.get(pk=subject_id).get_duration()
            # start_time of subject
            start_time = Subjects_info.objects.filter(id=subject_id).first().start_time
            # day of subject
            day = Subjects_info.objects.filter(id=subject_id).first().day
            day_start_times_used[day].append(start_time .strftime('%H:%M'))
            # store subject id that user choose in list to use in html
            user_sub.append(user.last().sub_id.id)

            for duration in range(int(durations)) :
                duration_time = datetime.datetime.combine(datetime.date.today(), start_time) + datetime.timedelta(hours=duration)
                day_start_times_used[day].append(duration_time.time().strftime('%H:%M'))

          

    elif 'delete_btn' in request.POST:
        # The delete button was clicked
        subject_id = request.POST.get('id')
        # Delete subject from user table using Django ORM
        User_subjects.objects.filter(user_id_id=user_id, sub_id_id=subject_id).delete()
        # duration_time of subject
        durations = Subjects_info.objects.get(pk=subject_id).get_duration()
        # start_time of subject
        start_time = Subjects_info.objects.filter(id=subject_id).first().start_time
        # day of subject
        day = Subjects_info.objects.filter(id=subject_id).first().day
        # remove time that has been regis in dict
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
                'Is_subjects_registed':Is_subjects_registed,
                'Is_subjects_passed':Is_subjects_passed,
                'Is_over_credit':Is_over_credit,
                'Is_day_overlapse':Is_day_overlapse,
                'Is_midterm_overlapse':Is_midterm_overlapse,
                'Is_final_overlapse':Is_final_overlapse,
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

def check_pass_subject(sub_id, u_id):
    total_subjects = Subjects.objects.filter(userid=u_id).values_list('real_subject_id', flat=True)
    select_sub = Subjects_info.objects.filter(id=sub_id).values_list('code', flat=True)

    if select_sub[0] in total_subjects:
        return False
    else:
        return True

def check_over_credit(user_id, sub_id):
    #เอา sub_id_id ที่select ไปแล้วทุกตัว
    subjects_id = User_subjects.objects.filter(user_id_id=user_id).values_list('sub_id_id', flat=True)
    print(subjects_id)
    all_credit = []
    for i in subjects_id:
        if i is not None :
            #หาcreditวิชาของทุกวิชาที่เคยselect
            credit_sub = Subjects_info.objects.filter(id=i).values_list('credit', flat=True).first()
            all_credit.append(credit_sub)
            print(all_credit)
    
    total_credits = sum(all_credit)
    print(total_credits)
    subject_credits = Subjects_info.objects.filter(id=sub_id).values_list('credit', flat=True).first()
    credits_now = total_credits + subject_credits
    print(credits_now)
    maxcredit = 22
    if credits_now > maxcredit:
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

    

    if hour_endtime_1 <= hour_starttime_2 or  hour_endtime_2 <=  hour_starttime_1: 
        if hour_starttime_1 == hour_starttime_2 and  hour_endtime_2 ==  hour_endtime_1:           #ทั้งเวลาเริ่มและเวลาจบเท่ากันไม่ได้
            return False
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
        else:
            return True                                                                         #ชั่วโมงไม่ได้มีเท่ากันเลยก็ไม่เป็นไร
    else:
        return False                                                                            #ชั่วโมงซ้อนทับกัน
    
#function check วันเวลาเรียน
def check_study_day(sub_id,user_id):
      
    totalsubject = User_subjects.objects.filter(user_id_id=user_id).values_list('sub_id_id', flat=True)  #subject ทั้งหมด ที่ user ได้ select ไปแล้ว
    
    #เก็บ day,start time,end time ของแต่ละ subject ที่ user select ไปแล้ว
    day_subject_selected = []
    starttime_subject_selected = []
    endtime_subject_selected = []
    name_subject_selected = []

    if totalsubject != []:
        for x in totalsubject:
            if x is not None :
                day_subject_selected.append(Subjects_info.objects.filter(id=x).values_list("day",flat=True))
                name_subject_selected.append(Subjects_info.objects.filter(id=x).values_list("name",flat=True))

                starttime = Subjects_info.objects.filter(id=x).values_list("start_time",flat=True)[0]
                endtime = Subjects_info.objects.filter(id=x).values_list("end_time",flat=True)[0]
                
                starttime_subject_selected.append(starttime.strftime('%H:%M:%S'))
                endtime_subject_selected.append(endtime.strftime('%H:%M:%S'))
    
    #เก็บ  day,start time,end time ของวิชาที่ user ต้องการ select ตอนนี้
    day_subject_select = Subjects_info.objects.filter(id=sub_id).values_list("day",flat=True)
    subject_name_select = Subjects_info.objects.filter(id=sub_id).values_list("name",flat=True)[0]

    starttime = Subjects_info.objects.filter(id=sub_id).values_list("start_time",flat=True)[0]
    endtime = Subjects_info.objects.filter(id=sub_id).values_list("end_time",flat=True)[0]

    starttime_subject_select = starttime.strftime('%H:%M:%S')
    endtime_subject_select = endtime.strftime('%H:%M:%S')

    for  y in range(0,len(day_subject_selected)): 
        if day_subject_select[0][0] == day_subject_selected[y][0]:
            if Check_time_Overlapse(starttime_subject_selected[y],endtime_subject_selected[y],starttime_subject_select,endtime_subject_select):
                    return True
            else: 
                    return " Date-Time overlapse between {} and {} ".format(subject_name_select,name_subject_selected[y][0])
    return True

#function check วันเวลาสอบกลางภาค
def check_midterm_day(sub_id,user_id):
    #วิชาที่เคยเลือกไปแล้ว
    totalsubject = User_subjects.objects.filter(user_id_id=user_id).values_list('sub_id_id', flat=True)
    print(totalsubject)

    all_code_sub = []
    for sub in totalsubject:
        #หาcodeวิชาของทุกวิชาที่เคยselect
        code_sub = Subjects_info.objects.filter(id=sub).values_list('code', flat=True).first()
        if code_sub is not None :
            all_code_sub.append(code_sub)
            print(all_code_sub)

    day_mid = []
    starttime_mid = []
    endtime_mid = []
    subject_name = []

    for x in all_code_sub:
        if x is not None :
            day = Subjects_Test_Date.objects.filter(code=x).values_list('mid_numday', flat=True).first()
            if day is not None :
                day = day.strftime('%Y-%m-%d')
            day_mid.append(day)
            print(day_mid)

            name = Subjects_Test_Date.objects.filter(code=x).values_list('name', flat=True).first()
            subject_name.append(name)
            
            starttime = Subjects_Test_Date.objects.filter(code=x).values_list('mid_starttime', flat=True).first()
            if starttime is not None :
                starttime = starttime.strftime('%H:%M:%S')
            starttime_mid.append(starttime)
            print(starttime_mid)

            endtime = Subjects_Test_Date.objects.filter(code=x).values_list('mid_endtime', flat=True).first()
            if endtime is not None :
                endtime = endtime.strftime('%H:%M:%S')
            endtime_mid.append(endtime)
            print(endtime_mid)

    #วิชาที่กำลังจะเลือก
    code_select = Subjects_info.objects.filter(id=sub_id).values_list('code', flat=True).first()
    print(code_select)

    select_subject_name = []
    select_name = Subjects_Test_Date.objects.filter(code=code_select).values_list('name', flat=True).first()
    select_subject_name.append(select_name)


    select_day_mid = []
    select_day = Subjects_Test_Date.objects.filter(code=code_select).values_list('mid_numday', flat=True).first()
    if select_day is not None :
        select_day = select_day.strftime('%Y-%m-%d')
        select_day_mid.append(select_day)
        print(select_day_mid)

    select_starttime_mid = []
    select_starttime = Subjects_Test_Date.objects.filter(code=code_select).values_list('mid_starttime', flat=True).first()
    if select_starttime is not None :
        select_starttime = select_starttime.strftime('%H:%M:%S')
        select_starttime_mid.append(select_starttime)
        print(select_starttime_mid)

    select_endtime_mid = []
    select_endtime = Subjects_Test_Date.objects.filter(code=code_select).values_list('mid_endtime', flat=True).first()
    if select_endtime is not None :
        select_endtime = select_endtime.strftime('%H:%M:%S')
        select_endtime_mid.append(select_endtime)
        print(select_endtime_mid)

    #check
    print(day_mid)
    for y in all_code_sub:
        if y is not None and day_mid != [] and starttime_mid != [] and endtime_mid != []:
            for x in range(0,len(all_code_sub)):
                if x is not None and day_mid[x] is not None:
                    sub_selected = day_mid[x].split("-") #split day_fin
                    year,month,day = int(sub_selected[0]),int(sub_selected[1]),int(sub_selected[2])
                    print(year,month,day)
                    day_subject_selected = date(year,month,day)                                     # day จาก subject ที่ user ได้ select ไปแล้ว
                    starttime_subject_selected = starttime_mid[x]                                      # start time จาก subject ที่ user ได้ select ไปแล้ว
                    print(starttime_subject_selected)
                    endtime_subject_selected = endtime_mid[x]                                            # end time จาก subject ที่ user ได้ select ไปแล้ว
                    print(day_subject_selected,starttime_subject_selected,endtime_subject_selected)
                else:
                    day_subject_selected = None

                if select_day_mid != []:
                    sub_select = select_day_mid[0].split("-")
                    year,month,day = int(sub_select[0]),int(sub_select[1]),int(sub_select[2])
                    day_subject_select = date(year,month,day)                                       # dayจาก subject ที่ user ต้องการ select 
                    starttime_subject_select = select_starttime_mid[0]                                 # start time จาก subject ที่ user ต้องการ select 
                    endtime_subject_select = select_endtime_mid[0]                                  # end time จาก subject ที่ user ต้องการ select
                    print(day_subject_select,starttime_subject_select,endtime_subject_select)
                else:
                    day_subject_select = None
                
                if day_subject_select == None or day_subject_selected == None:
                    return True
                elif day_subject_select == day_subject_selected:
                    if Check_time_Overlapse(starttime_subject_selected,endtime_subject_selected,starttime_subject_select,endtime_subject_select):
                        return True
                    else: 
                            return " Midterm Examday overlapse between {} and {} ".format(select_subject_name[0],subject_name[x])
    return True

def check_final_day(sub_id,user_id):
    #วิชาที่เคยเลือกไปแล้ว
    totalsubject = User_subjects.objects.filter(user_id_id=user_id).values_list('sub_id_id', flat=True)
    print(totalsubject)

    all_code_sub = []
    for sub in totalsubject:
        #หาcodeวิชาของทุกวิชาที่เคยselect
        code_sub = Subjects_info.objects.filter(id=sub).values_list('code', flat=True).first()
        if code_sub is not None :
            all_code_sub.append(code_sub)
            print(all_code_sub)

    day_fin = []
    starttime_fin = []
    endtime_fin = []
    subject_name = []

    for x in all_code_sub:
        if x is not None :
            day = Subjects_Test_Date.objects.filter(code=x).values_list('fin_numday', flat=True).first()
            if day is not None:
                day = day.strftime('%Y-%m-%d')
            day_fin.append(day)
            print(day_fin)

            name = Subjects_Test_Date.objects.filter(code=x).values_list('name', flat=True).first()
            subject_name.append(name)
            
            starttime = Subjects_Test_Date.objects.filter(code=x).values_list('fin_starttime', flat=True).first()
            if starttime is not None:
                starttime = starttime.strftime('%H:%M:%S')
            starttime_fin.append(starttime)
            print(starttime_fin)

            endtime = Subjects_Test_Date.objects.filter(code=x).values_list('fin_endtime', flat=True).first()
            if endtime is not None:
                endtime = endtime.strftime('%H:%M:%S')
            endtime_fin.append(endtime)
            print(endtime_fin)

    #วิชาที่กำลังจะเลือก
    code_select = Subjects_info.objects.filter(id=sub_id).values_list('code', flat=True).first()
    print(code_select)

    select_subject_name = []
    select_name = Subjects_Test_Date.objects.filter(code=code_select).values_list('name', flat=True).first()
    select_subject_name.append(select_name)

    select_day_fin = []
    select_day = Subjects_Test_Date.objects.filter(code=code_select).values_list('fin_numday', flat=True).first()
    if select_day is not None:
        select_day = select_day.strftime('%Y-%m-%d')
        select_day_fin.append(select_day)
    print(select_day_fin)

    select_starttime_fin = []
    select_starttime = Subjects_Test_Date.objects.filter(code=code_select).values_list('fin_starttime', flat=True).first()
    if select_starttime is not None:
        select_starttime = select_starttime.strftime('%H:%M:%S')
        select_starttime_fin.append(select_starttime)
    print(select_starttime_fin)

    select_endtime_fin = []
    select_endtime = Subjects_Test_Date.objects.filter(code=code_select).values_list('fin_endtime', flat=True).first()
    if select_endtime is not None:
        select_endtime = select_endtime.strftime('%H:%M:%S')
        select_endtime_fin.append(select_endtime)
    print(select_endtime_fin)

    #check
    print(day_fin)
    for x in range(0,len(all_code_sub)):
        if day_fin != [] and day_fin[x] is not None :
            sub_selected = day_fin[x].split("-") #split day_fin
            year,month,day = int(sub_selected[0]),int(sub_selected[1]),int(sub_selected[2])
            print(year,month,day)
            day_subject_selected = date(year,month,day)                                     # day จาก subject ที่ user ได้ select ไปแล้ว
            starttime_subject_selected = starttime_fin[x]                                      # start time จาก subject ที่ user ได้ select ไปแล้ว
            print(starttime_subject_selected)
            endtime_subject_selected = endtime_fin[x]                                            # end time จาก subject ที่ user ได้ select ไปแล้ว
            print(day_subject_selected,starttime_subject_selected,endtime_subject_selected)
        else:
            day_subject_selected = None

        if select_day_fin != []:
            sub_select = select_day_fin[0].split("-")
            year,month,day = int(sub_select[0]),int(sub_select[1]),int(sub_select[2])
            day_subject_select = date(year,month,day)                                       # dayจาก subject ที่ user ต้องการ select 
            starttime_subject_select = select_starttime_fin[0]                                 # start time จาก subject ที่ user ต้องการ select 
            endtime_subject_select = select_endtime_fin[0]                                  # end time จาก subject ที่ user ต้องการ select
            print(day_subject_select,starttime_subject_select,endtime_subject_select)
        else:
            day_subject_select = None

        if day_subject_select == None or day_subject_selected == None:
            return True
        elif day_subject_select == day_subject_selected:
            if Check_time_Overlapse(starttime_subject_selected,endtime_subject_selected,starttime_subject_select,endtime_subject_select):
                return True
            else: 
                return " Final Examday overlapse between {} and {} ".format(select_subject_name[0],subject_name[x])
    return True 