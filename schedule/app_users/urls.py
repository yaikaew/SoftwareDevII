from django.urls import path , include
from . import views

urlpatterns = [
    path('login/', views.login_view ,name = 'login'),
    path('register/', views.signup_view, name = 'register'),
    path('logout/', views.logout_view , name = 'logout'),
    path("<int:user_id>/",views.user_page, name='user_page'),
]
