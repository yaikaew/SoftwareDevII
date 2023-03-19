from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date
from app_schedule.models import Subjects_info , User_subjects
from django.db.models import Q
from django.utils import timezone
import datetime
from django import template

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
    if 'select_btn' in request.POST:
        # The select button was clicked
        subject_id = request.POST.get('id')
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
               'days':days}
    
    return render(request, 'select_subject.html' , context)
