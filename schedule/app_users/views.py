from django.shortcuts import render , redirect
from django.contrib.auth.views import LoginView
from django.contrib.auth import authenticate, login

# Create your views here.
def home(request):
    return render(request, 'home.html')

def login_view(request):
    if request.method == 'POST':
        username = request.POST.get('username')
        password = request.POST.get('password')
        user = authenticate(request, username=username, password=password)
        if user is not None:
            login(request, user)
            return redirect('home') # replace 'home' with the name of your homepage URL pattern
        else:
            # show an error message
            error_message = "Invalid login credentials"
            return render(request, 'login.html', {'error_message': error_message})
    else:
        return render(request, 'login.html')

# def login(request):
#     return render(request, 'login.html')

from django.contrib.auth.views import LoginView

class MyLoginView(LoginView):
    template_name = 'login.html'
