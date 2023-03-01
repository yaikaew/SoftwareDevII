from django.shortcuts import render , redirect
from django.contrib.auth.views import LoginView
from django.contrib.auth import authenticate, login
from django.views.generic.edit import CreateView

# Create your views here.
def home(request):
    return render(request, 'home.html')

class MyLoginView(LoginView):
    template_name = 'login.html'



