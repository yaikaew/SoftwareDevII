from django.db import models

# Create your models here.
class Gpax(models.Model):
    gpaxid = models.AutoField(db_column='GpaxID', primary_key=True)  # Field name made lowercase.
    gpax = models.FloatField(db_column='GPAX')  # Field name made lowercase.

    class Meta:
        managed = False
        db_table = 'gpax'
