from django.shortcuts import render 
from django.contrib.auth.decorators import login_required
from django.contrib.auth.models import User

# Create your views here.
@login_required(login_url='login')
def schedule_view(request):
    return render(request, 'schedule.html')