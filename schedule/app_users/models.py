from django.db import models
from django.contrib.auth.models import User

# Create your models here.
class Gpax(models.Model):
    gpaxid = models.AutoField(db_column='GpaxID', primary_key=True)  # Field name made lowercase.
    gpax = models.FloatField(db_column='GPAX')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'gpax'

class Gpa(models.Model):
    gpaid = models.AutoField(db_column='GpaID', primary_key=True)  # Field name made lowercase.
    gpa = models.FloatField(db_column='GPA')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'gpa'

class Subjects(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    real_subject_id = models.CharField(max_length=200)
    subject_name = models.CharField(max_length=200)
    credit = models.IntegerField()
    grade_char = models.CharField(max_length=200)
    grade_int = models.FloatField()
    year = models.CharField(max_length=200)
    semester = models.IntegerField()
    userid = models.IntegerField(db_column='UserID')
    gpaid = models.ForeignKey(Gpa, models.DO_NOTHING, db_column='GpaID_id')  # Field name made lowercase.
    gpaxid = models.ForeignKey(Gpax, models.DO_NOTHING, db_column='GpaxID_id')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'subjects'