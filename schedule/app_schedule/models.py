from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Subjects_info(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    code = models.CharField(max_length=255,null=True)
    name = models.CharField(max_length=255,null=True)
    day = models.CharField(max_length=255,null=True)
    credit = models.IntegerField(null=True)
    section = models.CharField(max_length=255,null=True)
    start_time = models.TimeField(null=True)
    end_time = models.TimeField(null=True)
    prof = models.CharField(max_length=255,null=True)

   
    #code,name,credit,section,day,start_time,end_time,prof

class User_subjects(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    user_id = models.ForeignKey(User, null=True, on_delete=models.CASCADE)
    sub_id = models.ForeignKey(Subjects_info, null=True, on_delete=models.CASCADE)