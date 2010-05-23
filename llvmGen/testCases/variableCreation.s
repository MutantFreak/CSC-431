

;; C helpers

declare void @print_val(i64)
declare void @halt_with_error(i64,i64)
declare void @halt_with_error_noval(i64)
declare void @halt_with_error_firstword(i64,i64)
declare void @halt_with_error_int(i64,i64)
declare i64  @find_slot(%slots*,i64)
declare i64  @try_to_set_slot(%slots*,i64,i64)
declare void @ding()
declare void @print_int(i64)

;; C primitives:

declare i64 @equal_prim(i64,i64)
declare i64 @and_prim(i64,i64)
declare i64 @or_prim(i64,i64)
declare i64 @not_prim(i64)
declare i64 @print_prim(i64)
declare i64 @flexiplus_prim(i64,i64)
declare i64 @fleximinus_prim(i64,i64)
declare i64 @flexitimes_prim(i64,i64)
declare i64 @flexidivide_prim(i64,i64)
declare i64 @flexilessthan_prim(i64,i64)
declare i64 @flexigreaterthan_prim(i64,i64)
declare i64 @flexilessequalthan_prim(i64,i64)
declare i64 @flexigreaterequalthan_prim(i64,i64)
declare i64 @stringLength_prim(i64)
declare i64 @subString_prim(i64,i64,i64)
declare i64 @stringAppend_prim(i64,i64)
declare i64 @stringEqualHuh_prim(i64, i64)
declare i64 @stringLessThanHuh_prim(i64, i64)
declare i64 @stringHuh_prim(i64)
declare i64 @floatHuh_prim(i64)
declare i64 @plainHuh_prim(i64)
declare i64 @closureHuh_prim(i64)
declare i64 @instanceof_prim(i64,i64)
declare i64 @sqrt_prim(i64)

;; the values in memory:

%obj =      type {i64, %slots*} ;; (this works for any memory val other than floats)
%closure =      type {i64, %slots*, %eframe*}
%strobj   = type {i64, %slots*, i8*}
%floatobj = type {i64, float}

;; environments and slots:

%eframe = type {%eframe*, i64, [0 x i64]}
%slots = type {%slots*, i64, i64}

@empty_env = constant %eframe undef
@empty_slots = constant %slots undef

%packed_args = type {i64, [0 x i64]}


@stringconst_0s = internal constant [9 x i8] c"bogusVal\00"



;; scheme_entry

define i64 @scheme_entry() {
	%reg_1 = call i64 @main_0(%eframe* @empty_env)
	ret i64 %reg_1
}



;; fun_dispatch

define i64 @fun_dispatch(i64 %fun_val,%packed_args* %args) {
	switch i64 %fun_val, label %L_regularcall_31 [i64 3, label %L_jump_to_flexilessthan_prim_2 i64 11, label %L_jump_to_flexilessequalthan_prim_3 i64 19, label %L_jump_to_flexigreaterthan_prim_4 i64 27, label %L_jump_to_flexigreaterequalthan_prim_5 i64 35, label %L_jump_to_equal_prim_6 i64 43, label %L_jump_to_flexiplus_prim_7 i64 51, label %L_jump_to_fleximinus_prim_8 i64 59, label %L_jump_to_flexitimes_prim_9 i64 67, label %L_jump_to_flexidivide_prim_10 i64 75, label %L_jump_to_and_prim_11 i64 83, label %L_jump_to_or_prim_12 i64 91, label %L_jump_to_not_prim_13 i64 99, label %L_jump_to_stringLength_prim_14 i64 107, label %L_jump_to_subString_prim_15 i64 115, label %L_jump_to_stringAppend_prim_16 i64 123, label %L_jump_to_stringEqualHuh_prim_17 i64 131, label %L_jump_to_stringLessThanHuh_prim_18 i64 139, label %L_jump_to_instanceof_prim_19 i64 147, label %L_jump_to_intHuh_prim_20 i64 155, label %L_jump_to_boolHuh_prim_21 i64 163, label %L_jump_to_floatHuh_prim_22 i64 171, label %L_jump_to_voidHuh_prim_23 i64 179, label %L_jump_to_stringHuh_prim_24 i64 187, label %L_jump_to_closureHuh_prim_25 i64 195, label %L_jump_to_plainHuh_prim_26 i64 203, label %L_jump_to_print_prim_27 i64 211, label %L_jump_to_readLine_prim_28 i64 219, label %L_jump_to_sqrt_prim_29 ]
L_jump_to_flexilessthan_prim_2:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexilessequalthan_prim_3:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterthan_prim_4:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterequalthan_prim_5:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_equal_prim_6:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexiplus_prim_7:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_fleximinus_prim_8:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexitimes_prim_9:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexidivide_prim_10:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_and_prim_11:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_or_prim_12:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_not_prim_13:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLength_prim_14:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_subString_prim_15:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringAppend_prim_16:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringEqualHuh_prim_17:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLessThanHuh_prim_18:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_instanceof_prim_19:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_intHuh_prim_20:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_boolHuh_prim_21:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_floatHuh_prim_22:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_voidHuh_prim_23:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringHuh_prim_24:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_closureHuh_prim_25:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_plainHuh_prim_26:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_print_prim_27:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_readLine_prim_28:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_sqrt_prim_29:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_regularcall_31:
	%reg_2 = and i64 %fun_val, 3
	%reg_3 = icmp eq i64 %reg_2, 1
	br i1 %reg_3, label %L_34, label %L_35
L_35:
	call void @halt_with_error_int(i64 5,i64 %fun_val) nounwind noreturn
	unreachable
L_34:
	%reg_4 = and i64 %fun_val, 18446744073709551612
	%reg_5 = inttoptr i64 %reg_4 to %closure*
	%reg_6 = getelementptr %closure* %reg_5, i32 0, i32 0
	%reg_7 = load i64* %reg_6
	%reg_8 = and i64 %reg_7, 3
	%reg_9 = icmp eq i64 %reg_8, 0
	br i1 %reg_9, label %L_36, label %L_37
L_37:
	call void @halt_with_error_firstword(i64 9,i64 %reg_7) nounwind noreturn
	unreachable
L_36:
	%reg_10 = getelementptr %closure* %reg_5, i32 0, i32 2
	%reg_11 = load %eframe** %reg_10
	switch i64 %reg_7, label %L_nomatch_33 [i64 0, label %L_jump_to_0_1 ]
L_nomatch_33:
	call void @halt_with_error(i64 3,i64 %reg_7) nounwind noreturn
	unreachable
L_wrongnumargs_32:
	call void @halt_with_error_noval(i64 4) nounwind noreturn
	unreachable
L_jump_to_0_1:
	%reg_12 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_13 = load i64* %reg_12
	%reg_14 = icmp eq i64 %reg_13, 0
	br i1 %reg_14, label %L_38, label %L_wrongnumargs_32
L_38:
	%reg_15 = getelementptr %closure* %reg_5, i32 0, i32 2
	%reg_16 = load %eframe** %reg_15
	%reg_17 = call i64 @main_0(%eframe* %reg_16)
	ret i64 %reg_17
}



;; method_dispatch

define i64 @method_dispatch(i64 %fun_val,%packed_args* %args,i64 %obj) {
	switch i64 %fun_val, label %L_regularcall_69 [i64 3, label %L_jump_to_flexilessthan_prim_40 i64 11, label %L_jump_to_flexilessequalthan_prim_41 i64 19, label %L_jump_to_flexigreaterthan_prim_42 i64 27, label %L_jump_to_flexigreaterequalthan_prim_43 i64 35, label %L_jump_to_equal_prim_44 i64 43, label %L_jump_to_flexiplus_prim_45 i64 51, label %L_jump_to_fleximinus_prim_46 i64 59, label %L_jump_to_flexitimes_prim_47 i64 67, label %L_jump_to_flexidivide_prim_48 i64 75, label %L_jump_to_and_prim_49 i64 83, label %L_jump_to_or_prim_50 i64 91, label %L_jump_to_not_prim_51 i64 99, label %L_jump_to_stringLength_prim_52 i64 107, label %L_jump_to_subString_prim_53 i64 115, label %L_jump_to_stringAppend_prim_54 i64 123, label %L_jump_to_stringEqualHuh_prim_55 i64 131, label %L_jump_to_stringLessThanHuh_prim_56 i64 139, label %L_jump_to_instanceof_prim_57 i64 147, label %L_jump_to_intHuh_prim_58 i64 155, label %L_jump_to_boolHuh_prim_59 i64 163, label %L_jump_to_floatHuh_prim_60 i64 171, label %L_jump_to_voidHuh_prim_61 i64 179, label %L_jump_to_stringHuh_prim_62 i64 187, label %L_jump_to_closureHuh_prim_63 i64 195, label %L_jump_to_plainHuh_prim_64 i64 203, label %L_jump_to_print_prim_65 i64 211, label %L_jump_to_readLine_prim_66 i64 219, label %L_jump_to_sqrt_prim_67 ]
L_jump_to_flexilessthan_prim_40:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexilessequalthan_prim_41:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterthan_prim_42:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterequalthan_prim_43:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_equal_prim_44:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexiplus_prim_45:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_fleximinus_prim_46:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexitimes_prim_47:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexidivide_prim_48:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_and_prim_49:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_or_prim_50:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_not_prim_51:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLength_prim_52:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_subString_prim_53:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringAppend_prim_54:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringEqualHuh_prim_55:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLessThanHuh_prim_56:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_instanceof_prim_57:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_intHuh_prim_58:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_boolHuh_prim_59:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_floatHuh_prim_60:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_voidHuh_prim_61:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringHuh_prim_62:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_closureHuh_prim_63:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_plainHuh_prim_64:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_print_prim_65:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_readLine_prim_66:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_sqrt_prim_67:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_regularcall_69:
	%reg_18 = and i64 %fun_val, 3
	%reg_19 = icmp eq i64 %reg_18, 1
	br i1 %reg_19, label %L_72, label %L_73
L_73:
	call void @halt_with_error_int(i64 5,i64 %fun_val) nounwind noreturn
	unreachable
L_72:
	%reg_20 = and i64 %fun_val, 18446744073709551612
	%reg_21 = inttoptr i64 %reg_20 to %closure*
	%reg_22 = getelementptr %closure* %reg_21, i32 0, i32 0
	%reg_23 = load i64* %reg_22
	%reg_24 = and i64 %reg_23, 3
	%reg_25 = icmp eq i64 %reg_24, 0
	br i1 %reg_25, label %L_74, label %L_75
L_75:
	call void @halt_with_error_firstword(i64 9,i64 %reg_23) nounwind noreturn
	unreachable
L_74:
	%reg_26 = getelementptr %closure* %reg_21, i32 0, i32 2
	%reg_27 = load %eframe** %reg_26
	switch i64 %reg_23, label %L_nomatch_71 [i64 0, label %L_jump_to_0_39 ]
L_nomatch_71:
	call void @halt_with_error(i64 3,i64 %reg_23) nounwind noreturn
	unreachable
L_wrongnumargs_70:
	call void @halt_with_error_noval(i64 4) nounwind noreturn
	unreachable
L_jump_to_0_39:
	%reg_28 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_29 = load i64* %reg_28
	%reg_30 = icmp eq i64 %reg_29, 0
	br i1 %reg_30, label %L_76, label %L_wrongnumargs_70
L_76:
	%reg_31 = getelementptr %closure* %reg_21, i32 0, i32 2
	%reg_32 = load %eframe** %reg_31
	%reg_33 = call i64 @main_0m(%eframe* %reg_32,i64 %obj)
	ret i64 %reg_33
}



;; main_0

{%eframe*, i64, [0 x i64]}

define i64 @main_0(%eframe* %env) {

  // make space for a new eframe
	%reg_34 = malloc {%eframe*, i64, [3 x i64]}, align 4
	// lets call {%eframe*, i64, [3 x i64]}* an eframe*
	// 3 because there are 3 local variables in this probram
	%reg_35 = bitcast {%eframe*, i64, [3 x i64]}* %reg_34 to %eframe*
	// load the reference to parent pointer to 36
	%reg_36 = getelementptr %eframe* %reg_35, i32 0, i32 0
  // the memory location defined by reg36 gets 
  // the current env, which is a eframe**
	store %eframe* %env, %eframe** %reg_36
	
	// put a reference to field 1 (slot) in env, into reg37
	%reg_37 = getelementptr %eframe* %reg_35, i32 0, i32 1
	// put 3, which is the number of variables in this function
	// into a space pointed at by reg37
	store i64 3, i64* %reg_37
	
	// reg38 gets a pointer to a piece of memory that's the size of strobj
	%reg_38 = malloc %strobj, align 4
	// put a reference to the first field of the strobj into reg_39
	%reg_39 = getelementptr %strobj* %reg_38, i32 0, i32 0
	// put 1 into the 0th field of that strobj
	store i64 1, i64* %reg_39
	// put a reference to the 1st field of the strobj (slots) into reg_40
	%reg_40 = getelementptr %strobj* %reg_38, i32 0, i32 1
  // put an empty slots into 1st field (%slots) of the strobj
	store %slots* @empty_slots, %slots** %reg_40
  // get the 2nd field of the strobj (the i8*) and put it into reg_41
	%reg_41 = getelementptr %strobj* %reg_38, i32 0, i32 2
	// 9 is how many characters there are, i8 is each character
	// store the i8* character array "bogusVal\0" into the i8* pointed at by reg_41
	store i8* getelementptr([9 x i8]* @stringconst_0s, i64 0, i64 0), i8** %reg_41
	// turn the ptr for the strobj into an int
	%reg_42 = ptrtoint %strobj* %reg_38 to i64
	// add the tag bits to the int and store that into reg_43
	// reg_43 now stores our finalized int pointer to the string (tag bits included)
	%reg_43 = or i64 %reg_42, 1
	
	// make reg_44 point to the 0th index in the array of variables that are stored in this frame	
	%reg_44 = getelementptr %eframe* %reg_35, i32 0, i32 2, i32 0
	// store the final product (which is int which is a ptr to strojb + 1 tag bit)
	// to register 44, (eframe (2 0))
	store i64 %reg_43, i64* %reg_44
	
	
	// make space for new floatobj
	%reg_45 = malloc %floatobj, align 4
	// put a reference to field 0 (an i64) of the the floatobj into reg_46
	%reg_46 = getelementptr %floatobj* %reg_45, i32 0, i32 0
	// put 3 into the address pointed at by reg_46 which is an i64 of floatobj
	store i64 3, i64* %reg_46
	// put a reference to field 1 (a float) of the floatobj into reg_47
	%reg_47 = getelementptr %floatobj* %reg_45, i32 0, i32 1
	// typecast the value of the double to a float, 
	// and store it into reg_47 (the float field of the floatobj)
	store float fptrunc(double 0x400921f9f01b866e to float), float* %reg_47
	// turn the ptr for the floatobj located in reg_45 to an i64 and put it into reg_48
	%reg_48 = ptrtoint %floatobj* %reg_45 to i64
	// add on the tag bits 01 to the int we just made and put it into reg_49
	%reg_49 = or i64 %reg_48, 1
	// put a reference to field (2 1) within the eframe in reg_35, into reg_50
	%reg_50 = getelementptr %eframe* %reg_35, i32 0, i32 2, i32 1
	// put our int result (including tag bits) into index 1 of the 
	// list of variables (reg_50) that are in the program
	store i64 %reg_49, i64* %reg_50
	
	// put a reference to field (2 1) (which is the float we just made) within the eframe in reg_35, into reg_51
	%reg_51 = getelementptr %eframe* %reg_35, i32 0, i32 2, i32 1
	// load the i64 from the address in reg_51, into reg_52
	%reg_52 = load i64* %reg_51
	// put a reference to field (2 2) (which is the destination) within the eframe in reg_35, into reg_51
	%reg_53 = getelementptr %eframe* %reg_35, i32 0, i32 2, i32 2
	// store the i64 we retrieved from memory, into reg_53
	store i64 %reg_52, i64* %reg_53
	// return statements
	ret i64 63
L_77:
	ret i64 63
}



;; main_0m

define i64 @main_0m(%eframe* %env,i64 %this) {
	%reg_54 = malloc {%eframe*, i64, [3 x i64]}, align 4
	%reg_55 = bitcast {%eframe*, i64, [3 x i64]}* %reg_54 to %eframe*
	%reg_56 = getelementptr %eframe* %reg_55, i32 0, i32 0
	store %eframe* %env, %eframe** %reg_56
	%reg_57 = getelementptr %eframe* %reg_55, i32 0, i32 1
	store i64 3, i64* %reg_57
	%reg_58 = malloc %strobj, align 4
	%reg_59 = getelementptr %strobj* %reg_58, i32 0, i32 0
	store i64 1, i64* %reg_59
	%reg_60 = getelementptr %strobj* %reg_58, i32 0, i32 1
	store %slots* @empty_slots, %slots** %reg_60
	%reg_61 = getelementptr %strobj* %reg_58, i32 0, i32 2
	store i8* getelementptr([9 x i8]* @stringconst_0s, i64 0, i64 0), i8** %reg_61
	%reg_62 = ptrtoint %strobj* %reg_58 to i64
	%reg_63 = or i64 %reg_62, 1
	%reg_64 = getelementptr %eframe* %reg_55, i32 0, i32 2, i32 0
	store i64 %reg_63, i64* %reg_64
	%reg_65 = malloc %floatobj, align 4
	%reg_66 = getelementptr %floatobj* %reg_65, i32 0, i32 0
	store i64 3, i64* %reg_66
	%reg_67 = getelementptr %floatobj* %reg_65, i32 0, i32 1
	store float fptrunc(double 0x400921f9f01b866e to float), float* %reg_67
	%reg_68 = ptrtoint %floatobj* %reg_65 to i64
	%reg_69 = or i64 %reg_68, 1
	%reg_70 = getelementptr %eframe* %reg_55, i32 0, i32 2, i32 1
	store i64 %reg_69, i64* %reg_70
	%reg_71 = getelementptr %eframe* %reg_55, i32 0, i32 2, i32 1
	%reg_72 = load i64* %reg_71
	%reg_73 = getelementptr %eframe* %reg_55, i32 0, i32 2, i32 2
	store i64 %reg_72, i64* %reg_73
	ret i64 63
L_78:
	ret i64 63
}

