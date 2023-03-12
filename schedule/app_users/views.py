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
    

