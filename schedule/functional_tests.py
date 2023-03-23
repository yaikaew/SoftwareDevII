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

    '''#login select search delete logout
    def test_basic_functionality(self):  
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
        time.sleep(1)

        #ดูว่า username ใช่ของเราไช่ไหม
        user = self.browser.find_element(By.XPATH, '/html/body/nav/form/nav/div[1]')
        self.assertIn('sopon888', user.text)

        time.sleep(1)

        #ไปหน้า select
        select_subjectbtn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/nav/div[2]/a[1]').click()
        time.sleep(2)
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/select/') #check ใช่หน้า select ใช่ไหมจากurl
        
        #เสิร์ชหาวิชาที่จะเลือก ในที่นี้คือ database
        #ไปที่คำว่า "ค้นหาวิชา"
        search = self.browser.find_element(By.XPATH, '//*[@id="section-a"]/h1')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",search)
        #เห็นช่องค้นหา
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        #ใส่คำว่าชื่อวิชาที่อยากจะค้นหา
        search_box.send_keys('database') 
        time.sleep(1)
        search_box.send_keys(Keys.ENTER)
        time.sleep(1)

        #เลื่อนไปดูว่าค้นหาเจอใช่ไหม
        searchbtn = self.browser.find_element(By.XPATH, '//*[@id="section-d"]')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",searchbtn)
        time.sleep(2)
        #ดูว่าเจอจริงๆใช่ไหม
        find_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn('DATABASE SYSTEMS', find_database.text)
        
        #ลองกดเลือกวิชา
        select_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER)
        time.sleep(1)
        #ดูว่าขึั้นในตารางแล้วใช่ไหม
        data_in_table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/span')
        self.assertIn('DATABASE SYSTEMS', data_in_table.text)

        #ไปหน้า schedule เพื่อดูว่าวิชาที่เลือกขึ้นในตารางแล้ว
        schedule_btn = self.browser.find_element(By.XPATH, '/html/body/nav/form/div/div[2]/a[2]').click()
        time.sleep(1)
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/schedule/')
        #ดูว่ามีวิชา DATABASE ขึั้นในตารางของหน้า schedule แล้วใช่ไหม
        data_in_table2 = self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/span')
        self.assertIn('DATABASE SYSTEMS', data_in_table2.text)
        #เลื่อนลงไปดูข้อมูลวัน เวลาสอบ
        info_exam = self.browser.find_element(By.XPATH, '//*[@id="section-a"]/h1')
        self.browser.execute_script("arguments[0].scrollIntoView(true);",info_exam)
        database_exam = self.browser.find_element(By.XPATH, '//*[@id="section-a"]/div')
        self.assertIn('Final', database_exam.text)
        time.sleep(1)

        #ย้อนกลับไปหน้า select เพื่อลบวิชา database
        select_subjectbtn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/div/div[2]/a[1]').click()
        time.sleep(1)
        #ลบวิชา database
        del_database = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/form/button').click()
        time.sleep(1)
        #ตรวจสอบว่าลบไปแล้วจริงๆ
        table = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table')
        self.assertNotIn('DATABASE SYSTEMS', table.text)

        #logout ออกจากระบบ
        logout = self.browser.find_element(By.XPATH,'/html/body/nav/form/div/div[2]/button').click()
        time.sleep(1)
        #ตรวจสอบว่าlogoutแล้วจริงๆ
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/')'''

    def test_collision_subject(self):  
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
        time.sleep(1)

        #ไปหน้า select
        select_subjectbtn =  self.browser.find_element(By.XPATH,'/html/body/nav/form/nav/div[2]/a[1]').click()
        time.sleep(1)
        self.assertEqual(self.browser.current_url, 'http://127.0.0.1:8000/select/') 

        #ดูว่าถ้าลงวิชาที่มีเวลาเรียน จะต้องลงวิชาที่ 2 ไม่ได้
        sub1 = 'DATABASE'
        sub2 = 'ECONOMY'
        #ค้นหาวิชา database + เลือก + ดูว่าขึ้นในตาราง
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box.send_keys(sub1 + Keys.ENTER)
        find_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn(sub1, find_database.text)
        select_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        database_in_table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/span')
        self.assertIn(sub1, database_in_table.text)

        #ค้นหาวิชา economy + เลือก + ดูว่าไม่ขึ้นในตาราง และมีข้อความแจ้งว่าเวลาชนกัน
        search_box2 = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box2.send_keys(sub2 + Keys.ENTER)
        find_economy = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn(sub2, find_economy.text)
        select_economy = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table')
        self.assertNotIn(sub2, table.text)
        overlapse = self.browser.find_element(By.XPATH, '/html/body/div/div/div/h2')
        self.assertIn('overlapse', overlapse.text)
        del_database = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/form/button').click()

        #ดูว่าถ้าลงวิชาที่มีเวลาสอบกลางภาคตรงกัน จะต้องลงวิชาที่ 2 ไม่ได้
        sub3 = 'MAN'
        sub4 = 'ECONOMICS'
        #ค้นหาวิชา MAN AND SOCIETY + เลือก + ดูว่าขึ้นในตาราง
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box.send_keys(sub3 + Keys.ENTER)
        find_man = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn(sub3, find_man.text)
        select_man = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        man_in_table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table')
        self.assertIn(sub3, man_in_table.text)

        #ค้นหาวิชา ECONOMICS FOR INDIVIDUAL DEV + เลือก + ดูว่าไม่ขึ้นในตาราง และมีข้อความแจ้งว่าเวลาชนกัน
        search_box2 = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box2.send_keys(sub4 + Keys.ENTER)
        find_economics = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn(sub4, find_economics.text)
        select_economics = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table') 
        time.sleep(3)
        self.assertNotIn(sub4, table.text)
        overlapse = self.browser.find_element(By.XPATH, '/html/body/div/div/div/h2')
        self.assertIn('overlapse', overlapse.text)
        del_man = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[3]/td[7]/form/button').click()

        #ดูว่าถ้าลงวิชาที่มีเวลาสอบกลางภาคตรงกัน จะต้องลงวิชาที่ 2 ไม่ได้
        sub1 = 'DATABASE'
        sub5 = '010123128' #computer network lab
        #ค้นหาวิชา DATABASE + เลือก + ดูว่าขึ้นในตาราง
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box.send_keys(sub1 + Keys.ENTER)
        find_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[2]')
        self.assertIn(sub1, find_database.text)
        select_database = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        database_in_table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/span')
        self.assertIn(sub1, database_in_table.text)

        #ค้นหาวิชา COMPUTER NETWORKS LABORATORY  + เลือก + ดูว่าไม่ขึ้นในตาราง และมีข้อความแจ้งว่าเวลาชนกัน
        search_box2 = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box2.send_keys(sub5 + Keys.ENTER)
        find_com = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div')
        self.assertIn(sub5, find_com.text)
        select_com = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table')
        self.assertNotIn(sub5, table.text)
        overlapse = self.browser.find_element(By.XPATH, '/html/body/div/div/div/h2')
        self.assertIn('overlapse', overlapse.text)
        del_database = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/form/button').click()

        #ค้นหาวิชา CIRCUITS AND ELECTRONICS  + เลือก + ดูว่าไม่ขึ้นในตาราง และมีข้อความแจ้งว่าเรียนผ่านแล้ว
        sub = '010113138'
        find_cir = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[1]')
        self.assertIn(sub, find_cir.text)
        select_cir = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table')
        self.assertNotIn(sub, table.text)
        passed = self.browser.find_element(By.XPATH, '/html/body/div/div/div/h2')
        self.assertIn('passed', passed.text)     

        #เช็คว่าถ้าลงไปแล้ว 1 วิชา แล้วมีการลงเซคอื่นของวิชานั้น จะต้องลงไม่ได้
        sub = '010113942'
        sub_pro = '01-CSP, 01-STN, 01-PSV, 01-NWS' #Project2

                #ค้นหาวิชา PROJECT II + เลือก + ดูว่าขึ้นในตาราง
        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box.send_keys(sub + Keys.ENTER)
        find_Project = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/h5[1]')
        self.assertIn(sub, find_Project.text)
        select_Project = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER) 
        Project_in_table =self.browser.find_element(By.XPATH, '/html/body/div/div/div/div/table/tbody/tr[3]/td[11]')
        self.assertIn(sub, Project_in_table.text)

        search_box = self.browser.find_element(By.XPATH, '//*[@id="section-c"]/input')
        search_box.send_keys(sub_pro + Keys.ENTER)
        find_Project = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div')
        self.assertIn(sub_pro, find_Project.text)
        select_Project = self.browser.find_element(By.XPATH, '//*[@id="section-b"]/div/form/button').send_keys(Keys.ENTER)
        registed = self.browser.find_element(By.XPATH, '/html/body/div/div/div/h2')
        self.assertIn('registed', registed.text)   

        del_database = self.browser.find_element(By.XPATH,'/html/body/div/div/div/div/table/tbody/tr[5]/td[3]/form/button').click()






if __name__ == '__main__':  
    unittest.main()