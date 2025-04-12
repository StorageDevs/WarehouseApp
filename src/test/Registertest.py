from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time

options = webdriver.ChromeOptions()
driver = webdriver.Chrome(options=options)
driver.set_window_size(1920, 1080)

# Nyisd meg a regisztrációs oldalt
driver.get("http://localhost:3000/register")

# Várakozzunk, amíg a regisztrációs mezők láthatóvá válnak
wait = WebDriverWait(driver, 10)
userName_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@placeholder="Username"]')))
password_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@placeholder="Password"]')))
fullName_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@placeholder="Fullname"]')))
email_input = wait.until(EC.presence_of_element_located((By.XPATH, '//input[@placeholder="Email"]')))
register_button = driver.find_element(By.XPATH, '//button[text()="Register"]')

# Töltsd ki a regisztrációs mezőket
userName_input.send_keys('Karcsi01')
password_input.send_keys('Admin123@')
fullName_input.send_keys('Nagy Károly')
email_input.send_keys('karcsi@kkszki.hu')

driver.save_screenshot("RegisterPage.png")
time.sleep(5)

# Kattints a regisztrációs gombra
register_button.click()

# Várakozzunk, amíg a felhasználó átirányításra kerül a /login oldalra
wait.until(EC.url_contains("/login"))

# Képmentés a regisztrált állapot után
driver.save_screenshot("PostRegisterLoginPage.png")

# Ellenőrizzük, hogy sikeres regisztráció után a helyes oldalra irányítanak
assert "login" in driver.current_url, "User was not redirected to login page"

# Böngésző bezárása
time.sleep(2)
driver.quit()