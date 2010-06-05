#include <stdio.h>
#include <stdlib.h>

#define TRUE 1
#define FALSE 0

typedef struct cloStruct{
  long long a;
  long long b;
  struct cloStruct* c;
} cloStruct;

//C helpers
extern void print_val(long long a);
extern void halt_with_error(long long a, long long b);
extern void halt_with_error_noval(long long a);
extern void halt_with_error_firstword(long long a, long long b);
extern void halt_with_error_int(long long a, long long b);
//extern unsigned long long find_slot(%slots*,long long b);
//extern unsigned long long try_to_set_slot(%slots*, long long b, long long c);
extern void ding();
extern void print_int(long long a);

//C primitives:
extern long long add_prim(long long a, long long b);
extern long long equal_prim(long long a, long long b);
extern long long or_prim(long long a, long long b);
extern long long not_prim(long long a);
extern long long print_prim(long long a);
extern long long flexiplus_prim(long long a, long long b);
extern long long fleximinus_prim(long long a, long long b);
extern long long flexitimes_prim(long long a, long long b);
extern long long flexidivide_prim(long long a, long long b);
extern long long flexilessthan_prim(long long a, long long b);
extern long long flexigreaterthan_prim(long long a, long long b);
extern long long flexilessequalthan_prim(long long a, long long b);
extern long long flexigreaterequalthan_prim(long long a, long long b);
extern long long stringLength_prim(long long a);
extern long long subString_prim(long long a, long long b, long long c);
extern long long stringAppend_prim(long long a, long long b);
extern long long stringEqualHuh_prim(long long a, long long b);
extern long long stringLessThanHuh_prim(long long a, long long b);
extern long long stringHuh_prim(long long a);
extern long long floatHuh_prim(long long a);
extern long long plainHuh_prim(long long a);
extern long long closureHuh_prim(long long a);
extern long long instanceof_prim(long long a, long long b);
extern long long sqrt_prim(long long a);

//private helper
extern int isInt(long long a);
extern int isDouble(long long a);
extern int isBool(long long a);
extern int isVoid(long long a);
extern int isString(long long a);
extern int isClosure(long long a);


extern int getInt(long long a);
extern double getDouble(long long a);
extern int getBool(long long a);
extern char* getString(long long a);
extern cloStruct getClosure(long long a);

extern void print_val(long long a){

}
extern void halt_with_error(long long a, long long b){

}
extern void halt_with_error_noval(long long a){

}
extern void halt_with_error_firstword(long long a, long long b){

}
extern void halt_with_error_int(long long a, long long b){

}
//extern unsigned long long find_slot(%slots*,long long b);
//extern unsigned long long try_to_set_slot(%slots*, long long b, long long c);
extern void ding(){

}
extern void print_int(long long a){

}

extern long long add_prim(long long a, long long b){
  return a;
}
extern long long equal_prim(long long a, long long b){
  return a;
}
extern long long and_prim(long long a, long long b){
  return a & b;
}
extern long long or_prim(long long a, long long b){
  return a | b;
}
extern long long not_prim(long long a){
  //check to make sure it is bool
  if(isBool(a)){
    //get's the actual value
    if(getBool(a)){
      return 6;
    }
    else{
      return 14;
    }
  }
  fprintf(stderr,"not_prim error, not a boolean\n");
  exit(1);
}
extern long long print_prim(long long a){
  printf("a=%d",a);
  return a;
}
extern long long flexiplus_prim(long long a, long long b){
  return a;
}
extern long long fleximinus_prim(long long a, long long b){
  return a;
}
extern long long flexitimes_prim(long long a, long long b){
  return a;
}
extern long long flexidivide_prim(long long a, long long b){
  return a;
}
extern long long flexilessthan_prim(long long a, long long b){
  return a;
}
extern long long flexigreaterthan_prim(long long a, long long b){
  return a;
}
extern long long flexilessequalthan_prim(long long a, long long b){
  return a;
}
extern long long flexigreaterequalthan_prim(long long a, long long b){
  return a;
}
extern long long stringLength_prim(long long a){
  return a;
}
extern long long subString_prim(long long a, long long b, long long c){
  return a;
}
extern long long stringAppend_prim(long long a, long long b){
  return a;
}
extern long long stringEqualHuh_prim(long long a, long long b){
  return a;
}
extern long long stringLessThanHuh_prim(long long a, long long b){
  return a;
}
extern long long stringHuh_prim(long long a){
  return a;
}
extern long long floatHuh_prim(long long a){
  return a;
}
extern long long plainHuh_prim(long long a){
  return a;
}
extern long long closureHuh_prim(long long a){
  return a;
}
extern long long instanceof_prim(long long a, long long b){
  return a;
}
extern long long sqrt_prim(long long a){
  return a;
}


//--00 - int
//--01 - double
//--10 - bool or void
//-110 - bool
//0110 - FALSE bool
//1110 - TRUE bool
//-010 - void
//--11 - pointer
//01 11 - string (Clements)
//10 11 - closure (our decision)


extern int isInt(long long a){
  long long aCheck = 3 & a;
  if(aCheck == 0){
    return TRUE;
  }
  return FALSE;
}

extern int isBool(long long a){
  long long aCheck = 7 & a;
  if(aCheck == 6){
    return TRUE;
  }
  return FALSE;
}

extern int isVoid(long long a){
  long long aCheck = 7 & a;
  if(aCheck == 2){
    return TRUE;
  }
  return FALSE;
}

extern int isPointer(long long a){
  long long aCheck = 3 & a;
  if(aCheck == 3){
    return TRUE;
  }
  return FALSE;
}

extern int isDouble(long long a){
  long long aCheck = 3 & a;
  if(aCheck == 1){
    return TRUE;
  }

  return FALSE;
}

extern int isString(long long a){
  if(!isPointer(a)){
    return FALSE;
  }
  
  long long masked = (long long) a & (long long) ~3; 
  long long* ptr = (long long*) masked;
  long long data = *(ptr);
  data = data & 3;

  if(data != 1){
    return FALSE;
  }
  return TRUE;
}

extern int isClosure(long long a){
  if(!isPointer(a)){
    return FALSE;
  }
    
  long long masked = (long long) a & (long long) ~3;
  long long* ptr = (long long*) masked;
  long long data = *ptr;
  data = data & 3;

  if(data != 2){
    return FALSE;
  }
  return TRUE;  
}

extern int getInt(long long a){
  if(!isInt(a)){
    fprintf(stderr,"in getInt, this is not int!\n");
    exit(1);
  }
  return a/4;
}

extern double getDouble(long long a){
  if(!isDouble(a)){
    fprintf(stderr,"in getDouble, this is not double!\n");
    exit(1);
  }
  long long masked = (long long) a & (long long) ~3;
  long long* ptr = (long long*) masked;

  long long data = *ptr;
  return data;
}

extern int getBool(long long a){
  if(!isBool(a)){
    fprintf(stderr,"in getBool, this is not bool!\n");
    exit(1);
  }
  long long aCheck = 15 & a;
  if(aCheck == 6){
    return FALSE;
  }
  else{
    return TRUE;
  }
}

extern char* getString(long long a){
  if(!isString(a)){
    fprintf(stderr,"in getString, this is not string!\n");
    exit(1);
  }
  
  long long masked = (long long) a & (long long) ~3;
  long long* ptr = (long long*) masked;

  char* data = (char*) *(ptr+1);
  
  return data;
}

extern cloStruct getClosure(long long a){
  if(!isString(a)){
    fprintf(stderr,"in getString, this is not string!\n");
    exit(1);
  }
  
  long long masked = (long long) a & (long long) ~3;
  cloStruct* ptr = (cloStruct*) masked;
  
  return *ptr;
}

int main(){
  llvmMain();

  return 0;
}



