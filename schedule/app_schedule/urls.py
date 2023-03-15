from django.urls import path , include
from . import views

urlpatterns = [
    path('', views.schedule_view ,name = 'schedule'),
    path('about/',views.about, name='about'),
]
