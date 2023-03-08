from django.urls import path , include
from . import views

urlpatterns = [
    path('register/', views.signup_view, name = 'register'),
    path("<int:user_id>/",views.user_page, name='user_page'),
]
