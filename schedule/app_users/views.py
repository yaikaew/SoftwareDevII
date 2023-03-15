from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth import login , logout
from django.contrib.auth.forms import UserCreationForm , AuthenticationForm
from .models import Gpax , Subjects
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required


# Create your views here.
def home(request):
    data = Gpax.objects.all()
    context = {'data': data }
    return render(request, 'home.html',context)

def signup_view(request):
    if request.method == "POST" :
        form = UserCreationForm(request.POST)
        if form.is_valid():
            user = form.save()
            login(request,user)
            return redirect('user_page',user_id=user.pk)
    else :
        form = UserCreationForm()
    return render(request,'sign-up.html',{"form":form})
    
def login_view(request):
    if request.method == 'POST':
        form = AuthenticationForm(data=request.POST)
        if form.is_valid():
            user = form.get_user()
            login(request, user)
            return redirect('user_page',user_id=user.pk) # replace 'home' with the name of your homepage URL pattern
        
    else:
        form = AuthenticationForm()
    return render(request, 'login.html',{'form':form})

def logout_view(request) :
    if request.method == "POST" :
        logout(request)
        return redirect('home')
    
@login_required(login_url='/login')
def user_page(request, user_id):
    user = get_object_or_404(User, id=user_id)
    subject = Subjects.objects.filter(userid = user_id)
    return render(request, 'user_page.html', {'user': user ,'subject': subject})
    

import sqlite3
def check_credit(user_id,sub_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()

    # Execute the SELECT statement and retrieve the total sum of credits
    c.execute("SELECT SUM(app_schedule_subjects_info.credit) FROM app_schedule_subjects_info,app_schedule_user_subjects "+
              "WHERE app_schedule_user_subjects.user_id_id = ?  AND app_schedule_user_subjects.sub_id_id = app_schedule_subjects_info.ID ",(user_id,)) #เปลี่ยนชื่อตารางจาก subject เป็น ที่สร้างใหม่
    result = c.fetchone()

    # Get the total sum of credits from the result tuple
    total_credits = result[0]

    # Print the total sum of credits
    print("Total credits:", total_credits)

    # Execute the SELECT statement to retrieve the sum of credits for the specified app_select_subjects_test_date
    c.execute("SELECT credit FROM app_select_subjects_test_date WHERE code = ?", (sub_id,)) 
    result = c.fetchone()

    # Get the sum of credits for the specified subject from the result tuple
    subject_credits = result[0]

    # Print the sum of credits for the specified subject
    print(f"Subject {sub_id} credits:", subject_credits)

    # Close the connections
    conn.close()
    
    credits_now = total_credits + subject_credits

    if credits_now >22:
        return False
    else:
        return True
    
#function check เรียนไปแล้วหรือยัง (ตาราง subject)
import sqlite3

def check_pass_subject(sub_id,u_id):
    conn = sqlite3.connect("w3.db")
    c = conn.cursor()

    #เลือกวิชาทั้งหมดที่มีจาก subjects ของ user คนนั้น
    c.execute("SELECT real_subject_id FROM subjects WHERE UserID = ?", (u_id,))
    data = c.fetchall()
    print(data)

    c.execute("SELECT code FROM app_schedule_subjects_info WHERE ID = ?", (sub_id,))
    sub = c.fetchall()
    print(sub)

    # Close the connections
    conn.close()

    for i in sub:
        if i in data:
            return False
        else:
            return True