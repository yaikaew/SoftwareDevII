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
    
# class SelectSubject(TestCase):
    
#     def test_select_to_database():
