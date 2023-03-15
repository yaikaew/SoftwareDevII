from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date
from app_schedule.models import Subjects_info , User_subjects
from django.db.models import Q

# Create your views here.
@login_required(login_url='login')
def selects_subject_view(request):
    sub_date = Subjects_Test_Date.objects.all()
    sub_objects = Subjects_info.objects.all()
    user_id = request.user.id
    user = User_subjects.objects.filter(user_id_id = user_id)

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
    elif 'delete_btn' in request.POST:
        # The delete button was clicked
        subject_id = request.POST.get('id')
        # Delete subject from user table using Django ORM
        User_subjects.objects.filter(user_id_id=user_id, sub_id_id=subject_id).delete()

    context = {'sub_date':sub_date, 'sub_name':sub_name ,'sub_objects':sub_objects,'users':user}
    return render(request, 'select_subject.html' , context)

