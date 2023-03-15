
# Create your tests here.

from django.test import TestCase
from django.urls import reverse
from django.contrib.auth.models import User
from .views import *


# class HomePageTest(TestCase):
#     def test_home_page(self):
#         url = reverse('home')  # Replace 'home' with the name of your homepage URL pattern
#         response = self.client.get(url)
#         self.assertEqual(response.status_code, 200)

from django.test import TestCase
from app_users.models import Subjects
from .views import check_credit

class CheckSelectTestCase(TestCase):
    
    def test_check_credit(self):
        user_id = 1
        sub_id = "010113138"
        result = check_credit(user_id, sub_id)
        self.assertEqual(result,True)

    def test_pass_subject(self):
        user_id = 1
        sub_id = 602
        result = check_pass_subject(sub_id, user_id)
        self.assertEqual(result,False)

    def test_regis_subject(self):
        user_id = 1
        sub_id = 430
        result = check_regis_subject(sub_id, user_id)
        self.assertEqual(result,False)   


class LoginTestCase(TestCase):
    
    def setUp(self):
        self.user = User.objects.create_user(
            username='testuser',
            password='testpass'
        )

    def test_login(self):
        response = self.client.post(reverse('login'), {
            'username': 'testuser',
            'password': 'testpass'
        })
        self.assertEqual(response.status_code, 302)
        #self.assertRedirects(response, reverse('user_page',args=[self.user.pk]))

    def test_login_with_wrong_password(self):
        response = self.client.post(reverse('login'), {
            'username': 'testuser',
            'password': 'wrongpassword'
        })
        self.assertEqual(response.status_code, 200)
        self.assertContains(response, 'username or password not correct')

