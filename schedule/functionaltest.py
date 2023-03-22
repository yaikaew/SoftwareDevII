from selenium import webdriver
import unittest
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
import time


username = 'ampamp4a'
email = 'amp@amp.com'
password = '1234'

class NewVisitorTest(unittest.TestCase):  

    def setUp(self):  
        self.browser = webdriver.Chrome()

    def tearDown(self):  
        self.browser.quit()

    def test_regis(self):  
        
        #เป็ดหน้าเว็บ
        self.browser.get('http://127.0.0.1:8000/') 
        time.sleep(2)

        #เลือกปุ่ม Regis
        regis_button =  self.browser.find_element(By.XPATH,'//*[@id="section-b2"]/a[2]')
        regis_button.click()
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/users/register/')

        #กรอก Username
        input_user = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[1]/input')
        input_user.send_keys(username)
        time.sleep(2)

        #กรอก Email
        input_email = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[2]/input')
        input_email.send_keys(email)
        time.sleep(2)

        #กรอก Password
        input_password = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[3]/input')
        input_password.send_keys(password)
        time.sleep(2)

        #กรอก Re Password
        input_repassword = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[4]/input')
        input_repassword.send_keys(password +Keys.ENTER)
        time.sleep(2)      

        #มาที่หน้าของ user
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/users/2/')

        #กดปุ่มเพื่อไปที่หน้า About us
        Aboutus_btn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/nav/div[2]/a[3]')
        Aboutus_btn.click()
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/schedule/about/')       
        time.sleep(2)
    
if __name__ == '__main__':  
    unittest.main()

