from django.urls import path , include
from . import views
from .views import MyLoginView , MySignUpView

urlpatterns = [
    path('home/', views.home, name = 'home'),
    path('login/', MyLoginView.as_view() , name = 'login'),
    path('sign-up/', MySignUpView, name = 'sign-up'),
]
