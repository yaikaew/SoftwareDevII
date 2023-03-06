from django.urls import path , include
from . import views

urlpatterns = [
    path('home/', views.home, name = 'home'),
    path('login/', views.login_view , name = 'login'),
    path('logout/', views.logout_view , name = 'logout'),
    path('register/', views.signup_view, name = 'register'),
]
