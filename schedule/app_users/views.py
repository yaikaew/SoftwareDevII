from django.shortcuts import render , redirect
from django.contrib.auth import authenticate, login , logout
from django.contrib.auth.forms import UserCreationForm , AuthenticationForm

# Create your views here.
def home(request):
    return render(request, 'home.html')

def signup_view(request):
    if request.method == "POST" :
        form = UserCreationForm(request.POST)
        if form.is_valid():
            user = form.save()
            login(request,user)
            return redirect("home")
    else :
        form = UserCreationForm()
    return render(request,'sign-up.html',{"form":form})
    
def login_view(request):
    if request.method == 'POST':
        form = AuthenticationForm(data=request.POST)
        if form.is_valid():
            user = form.get_user()
            login(request, user)
            return redirect('home') # replace 'home' with the name of your homepage URL pattern
    else:
        form = AuthenticationForm()
        # show an error message
    return render(request, 'login.html',{'form':form})

def logout_view(request) :
    if request.method == "POST" :
        logout(request)
        return redirect('home')
    

