from django.shortcuts import render
from django.contrib.auth.views import LoginView

# Create your views here.
def home(request):
    return render(request, 'home.html')

# def login(request):
#     return render(request, 'login.html')

from django.contrib.auth.views import LoginView

class MyLoginView(LoginView):
    template_name = 'login.html'
