from django.shortcuts import render , redirect , get_object_or_404
from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from .models import Subjects_Test_Date

# Create your views here.
@login_required(login_url='login')
def selects_subject_view(request):
    sub_date = Subjects_Test_Date.objects.all()
    context = {'sub_date':sub_date}
    return render(request, 'select_subject.html' , context)