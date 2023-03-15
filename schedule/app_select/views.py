from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date
from app_schedule.models import Subjects_info
from django.db.models import Q

# Create your views here.
@login_required(login_url='login')
def selects_subject_view(request):
    sub_date = Subjects_Test_Date.objects.all()
    if 'q' in request.GET:
        search = request.GET['q']
        multiple_search = Q(Q(name__icontains=search) | Q(code__icontains=search) | Q(prof__icontains=search))
        sub_name = Subjects_info.objects.filter(multiple_search)
    else:
        sub_name = Subjects_info.objects.none()
    context = {'sub_date':sub_date, 'sub_name':sub_name}
    return render(request, 'select_subject.html' , context)


""" def search_sub(request):
    sub_name = Subjects_info.objects.all()
    context = {'sub_name':sub_name}
    return render(request, 'select_subject.html' , context) """


