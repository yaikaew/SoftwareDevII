from django.db import models

# Create your models here.

class Subjects_Test_Date(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    code = models.CharField(max_length=255,null=True)
    name = models.CharField(max_length=255,null=True)
    credit = models.IntegerField(null=True)
    mid_numday = models.DateField(null=True)
    mid_starttime = models.TimeField(null=True)
    mid_endtime = models.TimeField(null=True)
    fin_numday = models.DateField(null=True)
    fin_starttime = models.TimeField(null=True)
    fin_endtime = models.TimeField(null=True)
    class Meta:
        managed = True

# code,name,credit,mid_numday,mid_starttime,mid_endtime,fin_numday,fin_starttime,fin_endtime