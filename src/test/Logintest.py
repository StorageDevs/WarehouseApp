from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.action_chains import ActionChains
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time

options = webdriver.ChromeOptions()
driver = webdriver.Chrome(options=options)
driver.set_window_size(1920, 1080)

# Nyisd meg a login oldalt
driver.get("http://localhost:3000/login")
driver.save_screenshot("LoginPage.png")

# Várakozzunk, amíg a bejelentkezési mezők láthatóvá válnak
wait = WebDriverWait(driver, 10)
username_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@type="text"]')))
password_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@type="password"]')))
login_button = driver.find_element(By.XPATH, '//button[text()="Login"]')

# Töltsd ki a bejelentkezési mezőket
username_input.send_keys('Admin')
password_input.send_keys('@Dmin1234')
login_button.click()

# Várakozás, amíg a felhasználó átirányításra kerül a /home oldalra
wait.until(EC.url_contains("/home"))

# Képmentés a bejelentkezett állapot után
driver.save_screenshot("PostLoginHomePage.png")

# Ellenőrizzük, hogy sikeres bejelentkezés után a helyes oldalra irányítanak
assert "home" in driver.current_url, "User was not redirected to home page"

# Böngésző bezárása
time.sleep(2)
driver.quit()
