from django.db import models

# Create your models here.

class Subjects_info(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    subjects_name = models.CharField(max_length=255,null=True)
    section = models.CharField(max_length=255,null=True)
    day = models.CharField(max_length=255,null=True)
    time = models.TimeField()
    professor = models.CharField(max_length=255,null=True)