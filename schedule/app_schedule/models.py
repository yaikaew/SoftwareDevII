from django.db import models
from django.contrib.auth.models import User
from datetime import datetime, time

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
    
    def get_duration(self):
        if self.start_time and self.end_time:
            start_datetime = datetime.combine(datetime.today(), self.start_time)
            end_datetime = datetime.combine(datetime.today(), self.end_time)
            duration = (end_datetime - start_datetime).seconds / 3600  # duration in hours
            return round(duration, 2)
        else:
            return None


   
    #code,name,credit,section,day,start_time,end_time,prof

class User_subjects(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    user_id = models.ForeignKey(User, null=True, on_delete=models.CASCADE)
    sub_id = models.ForeignKey(Subjects_info, null=True, on_delete=models.CASCADE)

