from django.contrib.auth.decorators import login_required
from django.shortcuts import render 
from app_select.models import Subjects_Test_Date
from app_schedule.models import Subjects_info , User_subjects
from app_select.views import day_start_times_used,start_times


# Create your views here.
@login_required(login_url='login')
def schedule_view(request):
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
    
    # The select button was clicked
    subject_id = request.POST.get('id')
    # insert name into user table using Django ORM
    User_subjects.objects.create(user_id_id=user_id, sub_id_id=subject_id)

    context = {'sub_date':sub_date, 
               'sub_objects':sub_objects,
               'users':user,
               'start_times':start_times,
               'day_start_times_used':day_start_times_used,
               'days':days}
    
    return render(request, 'schedule.html' , context)

def about(request):
    return render(request, 'about.html')