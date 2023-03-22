from selenium import webdriver
import unittest
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
import time


username = 'sopon888'
password = '8888'
class NewVisitorTest(unittest.TestCase):  

    def setUp(self):  
        self.browser = webdriver.Chrome()

    def tearDown(self):  
        self.browser.quit()

    def test_login_and_select(self):  
        #เปิดเว็บ 
        self.browser.get('http://127.0.0.1:8000/') 
        time.sleep(1)
        #ไปหน้า login
        login = self.browser.find_element(By.XPATH, '//*[@id="section-b2"]/a[1]').click()
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/users/login/')

        #ใส่username & password 
        input_user = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[1]/input')
        input_user.send_keys(username)
        time.sleep(1)
        input_password = self.browser.find_element(By.XPATH, '/html/body/div/div/form/div[2]/input')
        input_password.send_keys(password + Keys.ENTER)

        time.sleep(2)

        #ไปหน้า select
        select_subjectbtn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/nav/div[2]/a[1]').click()
        time.sleep(2)
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/select/')
        
        #เสิร์ชหาวิชาที่จะเลือก ในที่นี้คือ database
        search = self.browser.find_element(By.XPATH, '//*[@id="section-a"]/h1')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",search)
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        time.sleep(2)
        search_box.send_keys('database') 
        time.sleep(1)
        search_box.send_keys(Keys.ENTER)
        time.sleep(2)

        #เลื่อนไปดูว่าค้นหาเจอใช่ไหม
        searchbtn = self.browser.find_element(By.XPATH, '//*[@id="section-d"]')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",searchbtn)
        time.sleep(2)
        #ลองกดเลือกวิชา
        select_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER)
        time.sleep(2)

        #ไปหน้า schedule เพื่อดูว่าวิชาที่เลือกขึ้นในตารางแล้ว
        schedule_btn = self.browser.find_element(By.XPATH, '/html/body/nav/form/div/div[2]/a[2]').click()
        time.sleep(2)
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/schedule/')
        info_exam = self.browser.find_element(By.XPATH, '//*[@id="section-a"]/h1')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",info_exam)
        time.sleep(2)

        #ย้อนกลับไปหน้า select เพื่อลบวิชา database
        select_subjectbtn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/div/div[2]/a[1]').click()
        time.sleep(2)
        del_database = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/form/button').click()
        time.sleep(2)

        #logout ออกจากระบบ
        logout = self.browser.find_element(By.XPATH,'/html/body/nav/form/div/div[2]/button').click()
        time.sleep(2)
        
if __name__ == '__main__':  
    unittest.main()