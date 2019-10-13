/*********CODE ARDUINO GERANT LA POMPE ET LE VERIN*************/

#define RESET_CAM 8 //Pin permettantde reset l'Arduino de la caméra
#define INPUT1 2 //Pin de controle du pont en H
#define INPUT2 3
#define PUMP_TRANSISTOR 4 //Pin du transistor contrôlant l'alim de la pompe
#define CURRENT_PUMP A1 //Pin de la sortie de l'ampli d'instrumentation du courant de la pompe
#define CURRENT_ACT A4 //Idem pour le verin

#define THRESHOLD_PUSH 110 //Valeur que l'on ajoute à la conso moyenne du verin pour faire le seuil de consommation, à déterminer avec programme annexe
#define TIME_EXTEND 6000  //Temps max d'avancement
#define TIME_RETRACT 7000 //Temps max de retractaction
#define THRESHOLD_ASIC 90  //Valeur que l'écart min-max doit dépasser pour que l'on considère que la pompe tient l'ASIC, à déterminer avec programme annexe
#define SHUNT_PUMP_LOW 150 //Valeur min et max pour que le retour en conso de la pompe soit pris en compte
#define SHUNT_PUMP_HIGH 700
#define TIMEOUT_PUMP 8000 //Timeout en ms pour l'interrogation de la pompe

int shunt_pump = 0, shunt_act = 0, i = 0, somme_pompe = 0;
long int somme = 0, somme2 = 0, j = 0, k = 0;
bool stringComplete = false, is_to = false, is_retry_good = false, is_retry_bad = false;
int mini = 1023, maxi = 0;
unsigned long int currentMil = 0, previousMil = 0;
String inputString;

void setup() {
  // put your setup code here, to run once:
  pinMode(RESET_CAM, OUTPUT);
  pinMode(CURRENT_ACT, INPUT);
  pinMode(CURRENT_PUMP, INPUT);
  pinMode(INPUT1, OUTPUT);
  pinMode(INPUT2, OUTPUT);
  pinMode(PUMP_TRANSISTOR, OUTPUT);
  analogReference(EXTERNAL); //AREF connecté à 3V3, ref de l'ADC à 3V3 et plus à 5V
  digitalWrite(PUMP_TRANSISTOR, LOW); //Pompe et Verin inactif de base
  digitalWrite(RESET_CAM, LOW);
  ShutOff();
  Serial.begin(9600);
}

void loop() {


  if (Serial.available()) { //Récupère les donneés du port série
    inputString = Serial.readStringUntil('\n');
    stringComplete = true;
  }

  if (stringComplete && !is_to)
  {
    if (inputString.startsWith("RESET CAM"))
    { //Actionne la pompe à air
      digitalWrite(RESET_CAM, HIGH);
      delay(200); //Délai le temps que la pompe prenne possession de l'ASIC
      digitalWrite(RESET_CAM, LOW);
      delay(1500); //Le temps que la caméra soit de nouveau opérationnel
      Serial.println("OK RESET CAM\n");
    }
    else if (inputString.startsWith("PUMP 1"))
    { //Actionne la pompe à air
      digitalWrite(PUMP_TRANSISTOR, HIGH);
      delay(1000); //Délai le temps que la pompe prenne possession de l'ASIC
      Serial.println("OK PUMP 1\n");
    }
    else if (inputString.startsWith("PUMP 0"))
    { //Désactive la pompe à air
      digitalWrite(PUMP_TRANSISTOR, LOW);
      delay(1000);
      Serial.println("OK PUMP 0\n");
    }
    else if (inputString.startsWith("PUMP ?"))
    { //Déduis si la pompe tient l'ASIC
      //Relève la valeur min et max sur 20 mesures
      delay(800);
      while (true) {
        previousMil = millis(); //Prends le temps de réference pour le timeout
        while (i < 20) {

          currentMil = millis();
          if (currentMil - previousMil > TIMEOUT_PUMP) { //Si la requête dure trop longtemps, provoque un timeout
            digitalWrite(PUMP_TRANSISTOR, LOW);
            Serial.println("TIMEOUT POMPE\n");
            is_to = true;
            break;
          }

          shunt_pump = analogRead(CURRENT_PUMP);
          somme_pompe += shunt_pump;
          if (shunt_pump > SHUNT_PUMP_LOW and shunt_pump < SHUNT_PUMP_HIGH) {
            mini = (shunt_pump < mini ? shunt_pump : mini);
            maxi = (shunt_pump > maxi ? shunt_pump : maxi);
            i++;
          }
          delay(50);

        }
        Serial.print(somme_pompe / 20);
        Serial.print(" - ");
        Serial.print(mini);
        Serial.print(" - ");
        Serial.println(maxi);
        delay(150);

        if (!is_to)
          if (maxi - mini > THRESHOLD_ASIC) { //Si l'écart min/max est supérieur au seuil fixé, on considère que la ventouse tient l'ASIC
            if (maxi - mini < THRESHOLD_ASIC + 30 && !is_retry_good) //Si seuil passé de justesse, on refait une mesure
              is_retry_good = true;
            else {
              Serial.println("OK ASIC\n");
              break;
            }
          } else if (!is_retry_bad) { //Si seuil pas atteint, on refait une tentative
            is_retry_bad = true;
          } else { //Si 2 tentatives sont infructeuses
            Serial.println("OK !ASIC\n");
            break;
          }

        mini = 1023; maxi = 0; i = 0, somme_pompe = 0;
      }
      is_retry_good = false; is_retry_bad = false;
      mini = 1023; maxi = 0; i = 0, somme_pompe = 0;

    }
    else if (inputString.startsWith("ACTUATOR 1"))
    { //Descend le verin jusqu'à ce qu'il applique une force d'environ 100N
      i = 0; somme = 0; somme2 = 0; j = 0;
      previousMil = millis();
      while (true){
        currentMil = millis();
        if (currentMil - previousMil > TIME_EXTEND){
          setDirectionBW();
          Serial.println("TIMEOUT ACTUATOR");
          break;
        }
        setDirectionFW();
    
        shunt_act = analogRead(CURRENT_ACT);
        Serial.println(shunt_act);
    
        if (i < 50 && shunt_act > 200){
          somme += shunt_act;
          i++;
          Serial.print("somme : ");
          Serial.println(somme/i);
        }else if(shunt_act > 200){
          if (j < 20 && shunt_act > 200){
            somme2 += shunt_act;
            j++;
          }else{
            Serial.print("SEUIL : ");
            Serial.print((somme/50) + THRESHOLD_PUSH);
            Serial.print(", SOMME : ");
            Serial.println((somme2/20));
            if(somme2/20 > (somme/50) + THRESHOLD_PUSH){
              ShutOff();
              Serial.println("OK ACTUATOR 1");
              break;
            }
            j = 0;
            somme2 = 0;
         }
        }
        delay(10);
    
      }

    }
    else if (inputString.startsWith("ACTUATOR 0"))
    { //Remonte le verin
      if (fullyRetract())
        Serial.println("OK ACTUATOR 0\n");
    }
    else
    {
      Serial.println("TEST KO ARDUINO!\n");
    }
    stringComplete = false;
  }

  delay(50);
}

bool extend()
{
  setDirectionFW();
  previousMil = millis();
  while (true) {
    currentMil = millis();
    if (currentMil - previousMil > TIME_EXTEND) //Si le seuil de conso n'est pas atteind dans le temps imparti renvoi 0
      return 0;

    shunt_act = analogRead(CURRENT_ACT); //Fait la mesure de courant

    if (k < 50 && shunt_act > 200) { //Fait la moyenne des 50 premières mesures
      somme += shunt_act;
      k++;
      Serial.println((somme / k));
    } else {

      if (j < 20 && shunt_act > 200) { //Puis fait des moyennes sur 20 points en boucle
        somme2 += shunt_act;
        j++;
      } else {
        Serial.print("SEUIL : ");
        Serial.print((somme / 50) + THRESHOLD_PUSH);
        Serial.print(", SOMME : ");
        Serial.println((somme2 / 20));
        if ((somme2 / 20) > (somme / 50) + THRESHOLD_PUSH){ //Si une moyenne dépasse le seuil, renvoie 1
           k = 0; j = 0; somme2 = 0; somme = 0;
           return 1;
        }
      }
        j = 0;
        somme2 = 0;
    }
    delay(10);
  }
}

bool fullyRetract() {
  setDirectionBW();
  previousMil = millis();
  while (true) {
    currentMil = millis();
    if (currentMil - previousMil > TIME_RETRACT) {
      ShutOff();
      return 1;
    }
  }
}

void setDirectionFW() { //Fait avancer l'actionneur
  digitalWrite(INPUT1, LOW);
  digitalWrite(INPUT2, HIGH);
}

void setDirectionBW() { //Fait se rétracter l'actionneur
  digitalWrite(INPUT1, HIGH);
  digitalWrite(INPUT2, LOW);
}

void ShutOff() { //Arrête l'actionneur
  digitalWrite(INPUT1, LOW);
  digitalWrite(INPUT2, LOW);
}
