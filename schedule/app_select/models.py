from django.db import models

# Create your models here.

class Subjects_Test_Date(models.Model):
    id = models.AutoField(db_column='ID', primary_key=True)  # Field name made lowercase.
    name = models.CharField(max_length=255,null=True)
    mid = models.CharField(max_length=255,null=True)
    final = models.CharField(max_length=255,null=True)
    class Meta:
        managed = True
