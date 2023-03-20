from django.test import TestCase, Client
from django.urls import reverse
from django.contrib.auth.models import User

# Create your tests here.
class SelectPageTestCase(TestCase):

    def setUp(self):
        self.client = Client()
        self.user = User.objects.create_user(username='testuser', password='testpass')

    def test_secure_view_requires_login(self):
        response = self.client.get(reverse('selects_subject'))
        self.assertEqual(response.status_code, 302)
        self.assertRedirects(response, '/users/login/?next=/selects/test/')    

    def test_my_page_loads_correctly(self):
        self.client.login(username='testuser', password='testpass')
        response = self.client.get(reverse('selects_subject'))
        self.assertEqual(response.status_code, 200)

from app_schedule.models import Subjects_info , User_subjects
from app_select.models import Subjects_Test_Date
from .views import check_already_regis,check_over_credit,check_study_day,check_final_day,check_midterm_day
from datetime import time
class CheckSelect(TestCase):
    
    def setUp(self):
        self.user_id = 1
        self.User_subjects = User_subjects.objects.create(
            sub_id_id=1,
            user_id_id=1
        )
        #วิชาที่เคย regis ไปแล้ว
        self.Subjects_info = Subjects_info.objects.create(
            id = 1,
            code = '123',
            credit = '20',
            day = 'M',
            start_time = time(9, 30),
            end_time = time(12, 30)
        )

        self.Subjects_info = Subjects_info.objects.create(
            id = 2,
            code = '456',
            credit = '2',
            day = 'M',
            start_time = time(12, 30),
            end_time = time(15, 30)
        )

        self.Subjects_info = Subjects_info.objects.create(
            id = 3,
            code = '789',
            credit = '3',
            day = 'M',
            start_time = time(10, 30),
            end_time = time(12, 30)
        )

        self.Subjects_Test_Date = Subjects_Test_Date.objects.create(
            code = '123',
            mid_numday='2566-03-20',
            mid_starttime=time(9, 30),
            mid_endtime=time(12, 30),
            fin_numday='2566-05-20',
            fin_starttime=time(9, 30),
            fin_endtime=time(12, 30)
        )

        self.Subjects_Test_Date = Subjects_Test_Date.objects.create(
            code = '456',
            mid_numday='2566-03-20',
            mid_starttime=time(12, 30),
            mid_endtime=time(15, 30),
            fin_numday='2566-05-20',
            fin_starttime=time(12, 30),
            fin_endtime=time(15, 30)
        )

        self.Subjects_Test_Date = Subjects_Test_Date.objects.create(
            code = '789',
            mid_numday='2566-03-20',
            mid_starttime=time(10, 30),
            mid_endtime=time(12, 30),
            fin_numday='2566-05-20',
            fin_starttime=time(10, 30),
            fin_endtime=time(12, 30),
        )

    def testcheck_study_day_can_regis(self):
        #9:30-12:30 & 12:30-15:30 M
        result = check_study_day(2,self.user_id)
        self.assertEqual(result,True)

    def testcheck_study_day_can_not_regis(self):
        #9:30-12:30 & 10:30-12:30 M
        result = check_study_day(3,self.user_id)
        self.assertEqual(result,False)

    def testcheck_midterm_day_can_regis(self):
        #2566-03-20 9:30-12:30 & 2566-03-20 12:30-15:30
        result = check_midterm_day(2,self.user_id)
        self.assertEqual(result,True)

    def testcheck_midterm_day_can_not_regis(self):
        #2566-03-20 9:30-12:30 & 2566-03-20 10:30-12:30
        result = check_midterm_day(3,self.user_id)
        self.assertEqual(result,False)

    def testcheck_final_day_can_regis(self):
        #2566-05-20 9:30-12:30 & 2566-05-20 12:30-15:30
        result = check_final_day(2,self.user_id)
        self.assertEqual(result,True)

    def testcheck_final_day_can_not_regis(self):
        #2566-05-20 9:30-12:30 & 2566-05-20 10:30-12:30
        result = check_final_day(3,self.user_id)
        self.assertEqual(result,False)

    def testcheck_not_over_credit(self):
        #20 + 2 = 22
        result = check_over_credit(self.user_id,2)
        self.assertEqual(result,True)

    def testcheck_over_credit(self):
        #20 + 3 = 23
        result = check_over_credit(self.user_id,3)
        self.assertEqual(result,False)

    def testcheck_never_regis(self):
        result = check_already_regis(2,self.user_id)
        self.assertEqual(result,True)

    def testcheck_already_regis(self):
        result = check_already_regis(1,self.user_id)
        self.assertEqual(result,False)

    def tearDown(self):
        self.User_subjects.delete()
        self.Subjects_info.delete()
        self.Subjects_Test_Date.delete()