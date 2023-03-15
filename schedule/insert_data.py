import re
from bs4 import BeautifulSoup
from datetime import datetime
from app_select.models import Subjects_Test_Date
from app_schedule.models import Subjects_info

def get_test_date() :

    sub_code = None
    sub_credit = None
    sub_name = None
    mid_numday = None
    mid_starttime = None
    mid_endtime = None
    fin_numday = None
    fin_starttime = None
    fin_endtime = None

   # cursor = connection.cursor()

    first_Tag = 0

    # Read the HTML file
    with open("dataQuerySelector.html", "r",encoding="cp874") as f:
        content = f.read()

    # Use BeautifulSoup to parse the HTML content
    soup = BeautifulSoup(content, "html.parser")

    # Find the table in the HTML document
    table = soup.find_all("table")[4]


    for row in table.find_all('tr'):
    
        if first_Tag == 0 :

            cells = [cell.get_text().strip() for cell in row.find_all('td')]


            first_Tag = 1
            
        else :
            print("\n")
        # extract the text from each cell in the row

            cells = [cell.get_text().strip() for cell in row.find_all('td')]

            # clear pattern '\n\t\t3' that compound in tag
            cells[0] = cells[0].replace('\n\t\t', ' ')

            #print(cells[1])

            pattern1 = r"(\d+)([A-Za-z ]+)(\d+)"
            match1 = re.match(pattern1, cells[0])
            if match1:
                sub_code = match1.group(1)
                sub_name = match1.group(2)
                sub_credit = match1.group(3).strip()
                print("code : " + sub_code)
                print("name : " + sub_name)
                print("credit : " + sub_credit)

            mid = cells[1].split()

            if len(mid) > 0 :
                mid_numday = mid[1]
                mid_time = mid[2]
                mid_starttime,mid_endtime = mid_time.split("-")

                mid_numday = datetime.strptime(mid_numday, '%d/%m/%Y').date()
                mid_starttime = datetime.strptime(mid_starttime, '%H:%M').time()
                mid_endtime = datetime.strptime(mid_endtime, '%H:%M').time()
                
                # print("mid_numday : " + mid_numday)
                # print("mid_starttime : " + mid_starttime)
                # print("mid_endtime : " + mid_endtime)

            fin = cells[2].split()
        
            if len(fin) > 0 :
                fin_numday = fin[1]
                fin_time = fin[2]
                fin_starttime,fin_endtime = fin_time.split("-")

                fin_numday = datetime.strptime(fin_numday, '%d/%m/%Y').date()
                fin_starttime = datetime.strptime(fin_starttime, '%H:%M').time()
                fin_endtime = datetime.strptime(fin_endtime, '%H:%M').time()

                # print("fin_numday : " + fin_numday)
                # print("fin_starttime : " + fin_starttime)
                # print("fin_endtime : " + fin_endtime)
        
        Subjects_Test_Date.objects.create(code=sub_code,
                                          name=sub_name,
                                          credit=sub_credit,
                                          mid_numday=mid_numday,
                                          mid_starttime=mid_starttime,
                                          mid_endtime=mid_endtime,
                                          fin_numday=fin_numday,
                                          fin_starttime=fin_starttime,
                                          fin_endtime=fin_endtime)

def get_subject_info() :

    with open("data.html", "r", encoding="cp874") as f:
        data = f.read()

    soup = BeautifulSoup(data, 'html.parser')
    course = soup.find_all("table")[5]
    sub = course.find_all("tr")

    pattern = r"\(\d-\d\)"
    pattern_sub1 = r"\d+\s"
    pattern_sub2 = r"\s+\d+\(\d+-\d+\)"

    for i in range(1, len(sub)):
        subject = sub[i].find("b") 

        m = sub[i].find("tbody")
        sec = sub[i].find("td" ,{ "valign" : "top", "width" : "7%"})
        if m is not None:
            day = m.find("td" , {"width" : "7%"})
        time = sub[i].find("td" , {"width" : "25%"})
        prof = sub[i].find("td" , {"width" : "46%"})

        if subject is not None :
            a = subject.text
            temp_subject = subject.text.split()
            code_subject = temp_subject[0]
            credit = temp_subject[-1]
            credit_subject = re.sub(pattern, " " , credit)

            name1 = re.sub(pattern_sub1,"" , a)
            name_subject = re.sub(pattern_sub2," " , name1)
            print(" ")
            print("Code : " + code_subject)
            print("Name : " + name_subject)
            print("Credit : " + credit_subject)

        if sec is not None and day is not None and time is not None and prof is not None:
            sec_info = sec.text
            day_info = day.text
            Start_time_info = time.text.split("-")[0]
            End_time_info = time.text.split("-")[1]
            prof_info = prof.text
            print("Sec : " + sec_info)
            print("Day : " + day_info)
            print("Start time : " + Start_time_info)
            print("End time : " + End_time_info)
            print("Prof. : " + prof_info)

            Start_time_info = datetime.strptime(Start_time_info, '%H:%M').time()
            End_time_info = datetime.strptime( End_time_info, '%H:%M').time()

            Subjects_info.objects.create(code=code_subject,
                                              name=name_subject,
                                              credit=credit_subject,
                                              day = day_info,
                                              section=sec_info,
                                              start_time=Start_time_info,
                                              end_time=End_time_info,
                                              prof=prof_info)

