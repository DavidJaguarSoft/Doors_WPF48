char f;
int A =13;
int B =12;
int C =11;
int D=10;
int E=9;
int Sound=8;
void setup(){
  Serial.begin(9600);
  pinMode(D,OUTPUT);
  pinMode(E,OUTPUT);
  pinMode (A,OUTPUT);
   pinMode (B,OUTPUT); 
   pinMode (C,OUTPUT);
   pinMode(8,INPUT);
}
void loop(){

  if (Serial.available()>0){
    f= Serial.read();
    if(f==97){// a A on
      digitalWrite (A,HIGH);
     Serial.flush();
      Serial.println("A ON");
     tone(8,3000,150);
    }
    if (f==49){// 1 A off
      digitalWrite (A,LOW);
      Serial.flush();
      Serial.println("A OFF");
      tone(8,3000,150);
    }
       if(f==98){//b B on
      digitalWrite (B,HIGH);
      Serial.flush();
      Serial.println("B ON");
      tone(8,3000,150);
    }
    if (f==50){// 2 B off
      digitalWrite (B,LOW);
      Serial.flush();
      Serial.println("B OFF");
      tone(8,3000,150);
    }
      if(f==99){//c C on
      digitalWrite (C,HIGH);
      Serial.flush();
      Serial.println("C ON");
      tone(8,3000,150);
    }
    if (f==51){//3 C off
      digitalWrite (C,LOW);
      Serial.flush();
      Serial.println("C OFF");
      tone(8,3000,150);
    }
      if(f==100){//d D on
      digitalWrite (D,HIGH);
      Serial.flush();
      Serial.println("D ON");
      tone(8,3000,150);
    }
    if (f==52){// 4 D off
      digitalWrite (D,LOW);
      Serial.flush();
      Serial.println("D OFF");
      tone(8,3000,150);
    }
      if(f==101){//e E on
      digitalWrite (E,HIGH);
      Serial.flush();
      Serial.println("E ON");
      tone(8,3000,150);
    }
    if (f==53){//5 E off
      digitalWrite (E,LOW);
      Serial.flush();
      Serial.println("E OFF");
      tone(8,3000,150);
    }
    
    if (f==65){ //A All ON
    digitalWrite(A,HIGH);
    digitalWrite(B,HIGH);
    digitalWrite(C,HIGH);
    digitalWrite(D,HIGH);
    digitalWrite(E,HIGH);
    Serial.flush();
    Serial.println("Turn all on");
    tone(8,3500,150);
    delay(1000);
    tone(8,3500,150);
    delay(1000);
    tone(8,3500,150);
    }
     if (f==66){// B All OFF 
    digitalWrite(A,LOW);
    digitalWrite(B,LOW);
     digitalWrite(C,LOW);
    digitalWrite(D,LOW);
    digitalWrite(E,LOW);
    Serial.flush();
    Serial.println("Turn all off");
    tone(8,2000,150);
    delay(1000);
    tone(8,2000,150);   
    delay(1000);
    tone(8,2000,150);
    }
  }
}

