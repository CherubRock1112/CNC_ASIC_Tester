/*******CODE ARDUINO DE LA CAMERA**********/
// Basé sur le code Arducam OV5642


#include <Wire.h>
#include <ArduCAM.h>
#include <SPI.h>
#include <Servo.h>
#include "memorysaver.h"
//#if !(defined OV5642_MINI_5MP_PLUS)
//  #error Please select the hardware platform and camera module in the ../libraries/ArduCAM/memorysaver.h file
//#endif

#define BMPIMAGEOFFSET 66
#define RESET_ARDUINO 8

const char bmp_header[BMPIMAGEOFFSET] PROGMEM =
{
  0x42, 0x4D, 0x36, 0x58, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x42, 0x00, 0x00, 0x00, 0x28, 0x00,
  0x00, 0x00, 0x40, 0x01, 0x00, 0x00, 0xF0, 0x00, 0x00, 0x00, 0x01, 0x00, 0x10, 0x00, 0x03, 0x00,
  0x00, 0x00, 0x00, 0x58, 0x02, 0x00, 0xC4, 0x0E, 0x00, 0x00, 0xC4, 0x0E, 0x00, 0x00, 0x00, 0x00,
  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF8, 0x00, 0x00, 0xE0, 0x07, 0x00, 0x00, 0x1F, 0x00,
  0x00, 0x00
};
// set pin 7 as the slave select for the digital pot:
const int CS = 7;
bool is_header = false, stringComplete = false;
int mode = 0;
int enable = 1;
uint8_t start_capture = 0;
String inputString;
ArduCAM myCAM( OV5642, CS );
uint8_t read_fifo_burst(ArduCAM myCAM);


void setup() {
  // put your setup code here, to run once:
  pinMode(RESET_ARDUINO, OUTPUT);
  digitalWrite(RESET_ARDUINO, HIGH);
  uint8_t vid, pid;
  uint8_t temp;
#if defined(__SAM3X8E__)
  Wire1.begin();
  Serial.begin(115200);
#else
  Wire.begin();
  Serial.begin(921600);
#endif
  Serial.println(F("ACK CMD ArduCAM Start! END"));
  // set the CS as an output:
  pinMode(CS, OUTPUT);
  digitalWrite(CS, HIGH);
  // initialize SPI:
  SPI.begin();
  //Reset the CPLD
  myCAM.write_reg(0x07, 0x80);
  delay(100);
  myCAM.write_reg(0x07, 0x00);
  delay(100);
  while (1) {
    //Check if the ArduCAM SPI bus is OK
    myCAM.write_reg(ARDUCHIP_TEST1, 0x55);
    temp = myCAM.read_reg(ARDUCHIP_TEST1);
    if (temp != 0x55) {
      Serial.println(F("ACK CMD SPI interface Error! END"));
      delay(1000); continue;
    } else {
      Serial.println(F("ACK CMD SPI interface OK. END")); break;
    }
  }
  while (1) {
    //Check if the camera module type is OV5642
    myCAM.wrSensorReg16_8(0xff, 0x01);
    myCAM.rdSensorReg16_8(OV5642_CHIPID_HIGH, &vid);
    myCAM.rdSensorReg16_8(OV5642_CHIPID_LOW, &pid);
    if ((vid != 0x56) || (pid != 0x42)) {
      Serial.println(F("ACK CMD Can't find OV5642 module! END"));
      delay(1000); continue;
    }
    else {
      Serial.println(F("ACK CMD OV5642 detected. END")); break;
    }
  }
  //Change to JPEG capture mode and initialize the OV5642 module
  myCAM.set_format(JPEG);
  myCAM.InitCAM();

  myCAM.write_reg(ARDUCHIP_TIM, VSYNC_LEVEL_MASK);   //VSYNC is active HIGH
  myCAM.OV5642_set_JPEG_size(OV5642_320x240);
  delay(1000);
  myCAM.clear_fifo_flag();
  myCAM.write_reg(ARDUCHIP_FRAMES, 0x00);

  /*******CONFIGURATION DE LA RESOLUTION*************/
  myCAM.OV5642_set_JPEG_size(OV5642_2592x1944); delay(1000);
  Serial.println(F("ACK CMD switch to OV5642_2592x1944 END"));
}


void loop() {
  uint8_t temp = 0xff, temp_last = 0;
  bool is_header = false;

  if (Serial.available()) { //Récupère les données du port série
    inputString = Serial.readStringUntil('\n');
    stringComplete = true;
  }

  if (stringComplete)
  {
    if (inputString.startsWith("RESET ARDUINO"))
    { //Actionne la pompe à air
      Serial.println("OK test\n");
      digitalWrite(RESET_ARDUINO, LOW);
      delay(200); //Délai le temps que la pompe prenne possession de l'ASIC
      digitalWrite(RESET_ARDUINO, HIGH);
      delay(500); //Le temps que la caméra soit de nouveau opérationnel
      Serial.println("OK RESET ARDUINO\n");
    }
    else if (inputString.startsWith("Test"))
      Serial.println("Test OK!\n");
    else if (inputString.startsWith("CAM")) { //Prise d'une seule photo
      mode = 1;
      temp = 0xff;
      start_capture = 1;
      enable = 1;
    }
    else if (inputString.startsWith("STREAM")) { //Prise de photo en continue
      mode = 2;
      temp = 0xff;
      start_capture = 2;
      myCAM.OV5642_set_JPEG_size(OV5642_640x480); delay(500);
      Serial.println(F("ACK CMD CAM start video streaming. END"));
    }
    else
    {
      Serial.println("TEST KO CAM!\n");
    }
    stringComplete = false;
  }

  if (mode == 1)
  {
    if (enable == 1)    //autorisation de la caméra a prendre la photo
    {
      myCAM.flush_fifo();
      myCAM.clear_fifo_flag();
      //Start capture
      myCAM.start_capture();
      enable = 0;
    }
    if (myCAM.get_bit(ARDUCHIP_TRIG, CAP_DONE_MASK))
    {
      //Serial.println(F("ACK CMD CAM Capture Done. END")); delay(50);
      read_fifo_burst(myCAM);
      //Clear the capture done flag
      myCAM.clear_fifo_flag();
    }
  }
  else if (mode == 2)
  {
    while (1)
    {
      if (Serial.available()) { //Regarde le port série en attente de la commande d'arrêt de la capture
        inputString = Serial.readStringUntil('\n');
        stringComplete = true;
      }

      if (stringComplete)
      {
        if (inputString.startsWith("STOP")) //Si message stop reçu, arrête la capture
        {
          start_capture = 0;
          mode = 0;
          Serial.println(F("ACK CMD CAM stop video streaming. END"));
          myCAM.OV5642_set_JPEG_size(OV5642_2592x1944); delay(500);
          break;
        }
      }

      if (start_capture == 2)
      {
        myCAM.flush_fifo();
        myCAM.clear_fifo_flag();
        //Start capture
        myCAM.start_capture();
        start_capture = 0;
      }
      if (myCAM.get_bit(ARDUCHIP_TRIG, CAP_DONE_MASK))
      {
        uint32_t length = 0;
        length = myCAM.read_fifo_length();
        if ((length >= MAX_FIFO_SIZE) | (length == 0))
        {
          myCAM.clear_fifo_flag();
          start_capture = 2;
          continue;
        }
        myCAM.CS_LOW();
        myCAM.set_fifo_burst();//Set fifo burst mode
        temp =  SPI.transfer(0x00);
        length --;
        while ( length-- )
        {
          temp_last = temp;
          temp =  SPI.transfer(0x00);
          if (is_header == true)
          {
            Serial.write(temp);
          }
          else if ((temp == 0xD8) & (temp_last == 0xFF))
          {
            is_header = true;
            Serial.println(F("ACK IMG END"));
            Serial.write(temp_last);
            Serial.write(temp);
          }
          if ( (temp == 0xD9) && (temp_last == 0xFF) ) //If find the end ,break while,
            break;
          delayMicroseconds(15);
        }
        myCAM.CS_HIGH();
        myCAM.clear_fifo_flag();
        start_capture = 2;
        is_header = false;
      }
    }
  }
}

uint8_t read_fifo_burst(ArduCAM myCAM)
{
  uint8_t temp = 0, temp_last = 0;
  uint32_t length = 0;
  length = myCAM.read_fifo_length();
  //Serial.println(length, DEC);
  if (length >= MAX_FIFO_SIZE) //512 kb
  {
    //Serial.println(F("ACK CMD Over size. END"));
    return 0;
  }
  if (length == 0 ) //0 kb
  {
    //Serial.println(F("ACK CMD Size is 0. END"));
    return 0;
  }
  myCAM.CS_LOW();
  myCAM.set_fifo_burst();//Set fifo burst mode
  temp =  SPI.transfer(0x00);
  length --;
  while ( length-- )
  {
    temp_last = temp;
    temp =  SPI.transfer(0x00);
    if (is_header == true)
    {
      Serial.write(temp);
    }
    else if ((temp == 0xD8) & (temp_last == 0xFF))
    {
      is_header = true;
      //Serial.println(F("ACK IMG END"));
      Serial.write(temp_last);
      Serial.write(temp);
    }
    if ( (temp == 0xD9) && (temp_last == 0xFF) ) //If find the end ,break while,
      break;
    delayMicroseconds(15);
  }
  myCAM.CS_HIGH();
  digitalWrite(2, HIGH);
  is_header = false;
  return 1;
}
