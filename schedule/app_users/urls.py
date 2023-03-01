from django.urls import path , include
from . import views
from .views import MyLoginView

urlpatterns = [
    path('home/', views.login_view , name = 'home'),
    path('login/', MyLoginView.as_view() , name = 'login'),
]
