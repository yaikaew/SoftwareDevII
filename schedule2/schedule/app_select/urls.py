from django.urls import path , include
from . import views

urlpatterns = [
    path('', views.selects_subject_view ,name = 'selects_subject'),
]
