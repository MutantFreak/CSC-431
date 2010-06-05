

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





;; scheme_entry

define i64 @scheme_entry() {
	%reg_1 = call i64 @main_1(%eframe* @empty_env)
	ret i64 %reg_1
}



;; fun_dispatch

define i64 @fun_dispatch(i64 %fun_val,%packed_args* %args) {
	switch i64 %fun_val, label %L_regularcall_32 [i64 3, label %L_jump_to_flexilessthan_prim_3 i64 11, label %L_jump_to_flexilessequalthan_prim_4 i64 19, label %L_jump_to_flexigreaterthan_prim_5 i64 27, label %L_jump_to_flexigreaterequalthan_prim_6 i64 35, label %L_jump_to_equal_prim_7 i64 43, label %L_jump_to_flexiplus_prim_8 i64 51, label %L_jump_to_fleximinus_prim_9 i64 59, label %L_jump_to_flexitimes_prim_10 i64 67, label %L_jump_to_flexidivide_prim_11 i64 75, label %L_jump_to_and_prim_12 i64 83, label %L_jump_to_or_prim_13 i64 91, label %L_jump_to_not_prim_14 i64 99, label %L_jump_to_stringLength_prim_15 i64 107, label %L_jump_to_subString_prim_16 i64 115, label %L_jump_to_stringAppend_prim_17 i64 123, label %L_jump_to_stringEqualHuh_prim_18 i64 131, label %L_jump_to_stringLessThanHuh_prim_19 i64 139, label %L_jump_to_instanceof_prim_20 i64 147, label %L_jump_to_intHuh_prim_21 i64 155, label %L_jump_to_boolHuh_prim_22 i64 163, label %L_jump_to_floatHuh_prim_23 i64 171, label %L_jump_to_voidHuh_prim_24 i64 179, label %L_jump_to_stringHuh_prim_25 i64 187, label %L_jump_to_closureHuh_prim_26 i64 195, label %L_jump_to_plainHuh_prim_27 i64 203, label %L_jump_to_print_prim_28 i64 211, label %L_jump_to_readLine_prim_29 i64 219, label %L_jump_to_sqrt_prim_30 ]
L_jump_to_flexilessthan_prim_3:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexilessequalthan_prim_4:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterthan_prim_5:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterequalthan_prim_6:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_equal_prim_7:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexiplus_prim_8:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_fleximinus_prim_9:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexitimes_prim_10:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexidivide_prim_11:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_and_prim_12:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_or_prim_13:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_not_prim_14:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLength_prim_15:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_subString_prim_16:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringAppend_prim_17:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringEqualHuh_prim_18:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLessThanHuh_prim_19:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_instanceof_prim_20:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_intHuh_prim_21:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_boolHuh_prim_22:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_floatHuh_prim_23:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_voidHuh_prim_24:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringHuh_prim_25:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_closureHuh_prim_26:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_plainHuh_prim_27:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_print_prim_28:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_readLine_prim_29:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_sqrt_prim_30:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_regularcall_32:
   ;; and on the bits 3 (11) to isolate the bottom
   ;; throw away everything but the bottom 2 bits
	%reg_2 = and i64 %fun_val, 3
   ;; check if the bottom two bits were 01 (a pointer?)
	%reg_3 = icmp eq i64 %reg_2, 1
   ;; if they were, go to L_35, if not, go to L_36 (failure)
	br i1 %reg_3, label %L_35, label %L_36
L_36:
	call void @halt_with_error_int(i64 5,i64 %fun_val) nounwind noreturn
	unreachable
L_35:
   ;; acquire everything but the bottom two bits, which get turned to 0's.
	%reg_4 = and i64 %fun_val, 18446744073709551612
   ;; turn it into a closure
	%reg_5 = inttoptr i64 %reg_4 to %closure*
   ;; get a reference to the first field of the closuer and put it into reg_6
	%reg_6 = getelementptr %closure* %reg_5, i32 0, i32 0
   ;; pull the result out of that address and put it in reg_7
	%reg_7 = load i64* %reg_6
   ;; Throw away everything but the bottom two bits again
	%reg_8 = and i64 %reg_7, 3
   ;; make sure the bottom two bits are 00 (a function?)
	%reg_9 = icmp eq i64 %reg_8, 0
   ;; if the bottom two bits were 00, go to L_37, otherwise go to L_38 (failure)
	br i1 %reg_9, label %L_37, label %L_38
L_38:
	call void @halt_with_error_firstword(i64 9,i64 %reg_7) nounwind noreturn
	unreachable
L_37:
   ;; get field 2 out of the closure and put it into reg_10
	%reg_10 = getelementptr %closure* %reg_5, i32 0, i32 2
   ;; load the eframe out of reg_10
	%reg_11 = load %eframe** %reg_10
   ;; jump to labels for the actual functions.
	switch i64 %reg_7, label %L_nomatch_34 [i64 4, label %L_jump_to_1_1 i64 0, label %L_jump_to_0_2 ]
L_nomatch_34:
	call void @halt_with_error(i64 3,i64 %reg_7) nounwind noreturn
	unreachable
L_wrongnumargs_33:
	call void @halt_with_error_noval(i64 4) nounwind noreturn
	unreachable
;; Label for function 1 (main)
L_jump_to_1_1:
	%reg_12 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_13 = load i64* %reg_12
	%reg_14 = icmp eq i64 %reg_13, 0
	br i1 %reg_14, label %L_39, label %L_wrongnumargs_33
L_39:
	%reg_15 = getelementptr %closure* %reg_5, i32 0, i32 2
	%reg_16 = load %eframe** %reg_15
	%reg_17 = call i64 @main_1(%eframe* %reg_16)
	ret i64 %reg_17
;; Label for function 2 (our testFunction)
L_jump_to_0_2:
	%reg_18 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_19 = load i64* %reg_18
	%reg_20 = icmp eq i64 %reg_19, 1
	br i1 %reg_20, label %L_40, label %L_wrongnumargs_33
L_40:
	%reg_21 = getelementptr %packed_args* %args, i32 0, i32 1, i32 0
	%reg_22 = load i64* %reg_21
	%reg_23 = getelementptr %closure* %reg_5, i32 0, i32 2
	%reg_24 = load %eframe** %reg_23
	%reg_25 = call i64 @testFunction_0(%eframe* %reg_24,i64 %reg_22)
	ret i64 %reg_25
}



;; method_dispatch
;; switch for the value fun_val. L_regularcall_72 is the fall through case. A single case is "i64 3, label %L_jump_to_flexilessthan_prim_43"
define i64 @method_dispatch(i64 %fun_val,%packed_args* %args,i64 %obj) {
	switch i64 %fun_val, label %L_regularcall_72 [i64 3, label %L_jump_to_flexilessthan_prim_43 i64 11, label %L_jump_to_flexilessequalthan_prim_44 i64 19, label %L_jump_to_flexigreaterthan_prim_45 i64 27, label %L_jump_to_flexigreaterequalthan_prim_46 i64 35, label %L_jump_to_equal_prim_47 i64 43, label %L_jump_to_flexiplus_prim_48 i64 51, label %L_jump_to_fleximinus_prim_49 i64 59, label %L_jump_to_flexitimes_prim_50 i64 67, label %L_jump_to_flexidivide_prim_51 i64 75, label %L_jump_to_and_prim_52 i64 83, label %L_jump_to_or_prim_53 i64 91, label %L_jump_to_not_prim_54 i64 99, label %L_jump_to_stringLength_prim_55 i64 107, label %L_jump_to_subString_prim_56 i64 115, label %L_jump_to_stringAppend_prim_57 i64 123, label %L_jump_to_stringEqualHuh_prim_58 i64 131, label %L_jump_to_stringLessThanHuh_prim_59 i64 139, label %L_jump_to_instanceof_prim_60 i64 147, label %L_jump_to_intHuh_prim_61 i64 155, label %L_jump_to_boolHuh_prim_62 i64 163, label %L_jump_to_floatHuh_prim_63 i64 171, label %L_jump_to_voidHuh_prim_64 i64 179, label %L_jump_to_stringHuh_prim_65 i64 187, label %L_jump_to_closureHuh_prim_66 i64 195, label %L_jump_to_plainHuh_prim_67 i64 203, label %L_jump_to_print_prim_68 i64 211, label %L_jump_to_readLine_prim_69 i64 219, label %L_jump_to_sqrt_prim_70 ]
L_jump_to_flexilessthan_prim_43:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexilessequalthan_prim_44:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterthan_prim_45:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexigreaterequalthan_prim_46:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_equal_prim_47:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexiplus_prim_48:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_fleximinus_prim_49:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexitimes_prim_50:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_flexidivide_prim_51:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_and_prim_52:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_or_prim_53:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_not_prim_54:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLength_prim_55:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_subString_prim_56:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringAppend_prim_57:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringEqualHuh_prim_58:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringLessThanHuh_prim_59:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_instanceof_prim_60:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_intHuh_prim_61:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_boolHuh_prim_62:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_floatHuh_prim_63:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_voidHuh_prim_64:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_stringHuh_prim_65:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_closureHuh_prim_66:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_plainHuh_prim_67:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_print_prim_68:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_readLine_prim_69:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_jump_to_sqrt_prim_70:
	call void @halt_with_error_noval(i64 10) nounwind noreturn
	unreachable
L_regularcall_72:
	%reg_26 = and i64 %fun_val, 3
	%reg_27 = icmp eq i64 %reg_26, 1
	br i1 %reg_27, label %L_75, label %L_76
L_76:
	call void @halt_with_error_int(i64 5,i64 %fun_val) nounwind noreturn
	unreachable
L_75:
	%reg_28 = and i64 %fun_val, 18446744073709551612
	%reg_29 = inttoptr i64 %reg_28 to %closure*
	%reg_30 = getelementptr %closure* %reg_29, i32 0, i32 0
	%reg_31 = load i64* %reg_30
	%reg_32 = and i64 %reg_31, 3
	%reg_33 = icmp eq i64 %reg_32, 0
	br i1 %reg_33, label %L_77, label %L_78
L_78:
	call void @halt_with_error_firstword(i64 9,i64 %reg_31) nounwind noreturn
	unreachable
L_77:
	%reg_34 = getelementptr %closure* %reg_29, i32 0, i32 2
	%reg_35 = load %eframe** %reg_34
	switch i64 %reg_31, label %L_nomatch_74 [i64 4, label %L_jump_to_1_41 i64 0, label %L_jump_to_0_42 ]
L_nomatch_74:
	call void @halt_with_error(i64 3,i64 %reg_31) nounwind noreturn
	unreachable
L_wrongnumargs_73:
	call void @halt_with_error_noval(i64 4) nounwind noreturn
	unreachable
L_jump_to_1_41:
	%reg_36 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_37 = load i64* %reg_36
	%reg_38 = icmp eq i64 %reg_37, 0
	br i1 %reg_38, label %L_79, label %L_wrongnumargs_73
L_79:
	%reg_39 = getelementptr %closure* %reg_29, i32 0, i32 2
	%reg_40 = load %eframe** %reg_39
	%reg_41 = call i64 @main_1m(%eframe* %reg_40,i64 %obj)
	ret i64 %reg_41
L_jump_to_0_42:
	%reg_42 = getelementptr %packed_args* %args, i32 0, i32 0
	%reg_43 = load i64* %reg_42
	%reg_44 = icmp eq i64 %reg_43, 1
	br i1 %reg_44, label %L_80, label %L_wrongnumargs_73
L_80:
	%reg_45 = getelementptr %packed_args* %args, i32 0, i32 1, i32 0
	%reg_46 = load i64* %reg_45
	%reg_47 = getelementptr %closure* %reg_29, i32 0, i32 2
	%reg_48 = load %eframe** %reg_47
	%reg_49 = call i64 @testFunction_0m(%eframe* %reg_48,i64 %obj,i64 %reg_46)
	ret i64 %reg_49
}



;; main_1

define i64 @main_1(%eframe* %env) {
	%reg_50 = malloc {%eframe*, i64, [2 x i64]}, align 4
	%reg_51 = bitcast {%eframe*, i64, [2 x i64]}* %reg_50 to %eframe*
	%reg_52 = getelementptr %eframe* %reg_51, i32 0, i32 0
	store %eframe* %env, %eframe** %reg_52
	%reg_53 = getelementptr %eframe* %reg_51, i32 0, i32 1
	store i64 2, i64* %reg_53
	%reg_54 = malloc %closure, align 4
	%reg_55 = getelementptr %closure* %reg_54, i32 0, i32 0
	store i64 0, i64* %reg_55
	%reg_56 = getelementptr %closure* %reg_54, i32 0, i32 1
	store %slots* @empty_slots, %slots** %reg_56
	%reg_57 = getelementptr %closure* %reg_54, i32 0, i32 2
	store %eframe* %reg_51, %eframe** %reg_57
	%reg_58 = ptrtoint %closure* %reg_54 to i64
	%reg_59 = or i64 %reg_58, 1
	%reg_60 = getelementptr %eframe* %reg_51, i32 0, i32 2, i32 0
	store i64 %reg_59, i64* %reg_60
	%reg_61 = getelementptr %eframe* %reg_51, i32 0, i32 2, i32 0
	%reg_62 = load i64* %reg_61
	%reg_63 = malloc {i64, [1 x i64]}, align 4
	%reg_64 = getelementptr {i64, [1 x i64]}* %reg_63, i32 0, i32 0
	store i64 1, i64* %reg_64
	%reg_65 = getelementptr {i64, [1 x i64]}* %reg_63, i32 0, i32 1, i32 0
	store i64 60, i64* %reg_65
	%reg_66 = bitcast {i64, [1 x i64]}* %reg_63 to %packed_args*
	%reg_67 = call i64 @fun_dispatch(i64 %reg_62,%packed_args* %reg_66)
	%reg_68 = getelementptr %eframe* %reg_51, i32 0, i32 2, i32 1
	store i64 %reg_67, i64* %reg_68
	ret i64 63
L_81:
	ret i64 63
}



;; main_1m

define i64 @main_1m(%eframe* %env,i64 %this) {
	%reg_69 = malloc {%eframe*, i64, [2 x i64]}, align 4
	%reg_70 = bitcast {%eframe*, i64, [2 x i64]}* %reg_69 to %eframe*
	%reg_71 = getelementptr %eframe* %reg_70, i32 0, i32 0
	store %eframe* %env, %eframe** %reg_71
	%reg_72 = getelementptr %eframe* %reg_70, i32 0, i32 1
	store i64 2, i64* %reg_72
	%reg_73 = malloc %closure, align 4
	%reg_74 = getelementptr %closure* %reg_73, i32 0, i32 0
	store i64 0, i64* %reg_74
	%reg_75 = getelementptr %closure* %reg_73, i32 0, i32 1
	store %slots* @empty_slots, %slots** %reg_75
	%reg_76 = getelementptr %closure* %reg_73, i32 0, i32 2
	store %eframe* %reg_70, %eframe** %reg_76
	%reg_77 = ptrtoint %closure* %reg_73 to i64
	%reg_78 = or i64 %reg_77, 1
	%reg_79 = getelementptr %eframe* %reg_70, i32 0, i32 2, i32 0
	store i64 %reg_78, i64* %reg_79
	%reg_80 = getelementptr %eframe* %reg_70, i32 0, i32 2, i32 0
	%reg_81 = load i64* %reg_80
	%reg_82 = malloc {i64, [1 x i64]}, align 4
	%reg_83 = getelementptr {i64, [1 x i64]}* %reg_82, i32 0, i32 0
	store i64 1, i64* %reg_83
	%reg_84 = getelementptr {i64, [1 x i64]}* %reg_82, i32 0, i32 1, i32 0
	store i64 60, i64* %reg_84
	%reg_85 = bitcast {i64, [1 x i64]}* %reg_82 to %packed_args*
	%reg_86 = call i64 @fun_dispatch(i64 %reg_81,%packed_args* %reg_85)
	%reg_87 = getelementptr %eframe* %reg_70, i32 0, i32 2, i32 1
	store i64 %reg_86, i64* %reg_87
	ret i64 63
L_82:
	ret i64 63
}



;; testFunction_0

define i64 @testFunction_0(%eframe* %env,i64 %x_arg) {
	%reg_88 = malloc {%eframe*, i64, [2 x i64]}, align 4
	%reg_89 = bitcast {%eframe*, i64, [2 x i64]}* %reg_88 to %eframe*
	%reg_90 = getelementptr %eframe* %reg_89, i32 0, i32 0
	store %eframe* %env, %eframe** %reg_90
	%reg_91 = getelementptr %eframe* %reg_89, i32 0, i32 1
	store i64 2, i64* %reg_91
	%reg_92 = getelementptr %eframe* %reg_89, i32 0, i32 2, i32 0
	store i64 %x_arg, i64* %reg_92
	%reg_93 = getelementptr %eframe* %reg_89, i32 0, i32 2, i32 0
	%reg_94 = load i64* %reg_93
	%reg_95 = call i64 @flexiplus_prim(i64 %reg_94,i64 4)
	%reg_96 = getelementptr %eframe* %reg_89, i32 0, i32 2, i32 1
	store i64 %reg_95, i64* %reg_96
	%reg_97 = getelementptr %eframe* %reg_89, i32 0, i32 2, i32 1
	%reg_98 = load i64* %reg_97
	ret i64 %reg_98
L_83:
	ret i64 63
}



;; testFunction_0m

define i64 @testFunction_0m(%eframe* %env,i64 %this,i64 %x_arg) {
	%reg_99 = malloc {%eframe*, i64, [2 x i64]}, align 4
	%reg_100 = bitcast {%eframe*, i64, [2 x i64]}* %reg_99 to %eframe*
	%reg_101 = getelementptr %eframe* %reg_100, i32 0, i32 0
	store %eframe* %env, %eframe** %reg_101
	%reg_102 = getelementptr %eframe* %reg_100, i32 0, i32 1
	store i64 2, i64* %reg_102
	%reg_103 = getelementptr %eframe* %reg_100, i32 0, i32 2, i32 0
	store i64 %x_arg, i64* %reg_103
	%reg_104 = getelementptr %eframe* %reg_100, i32 0, i32 2, i32 0
	%reg_105 = load i64* %reg_104
	%reg_106 = call i64 @flexiplus_prim(i64 %reg_105,i64 4)
	%reg_107 = getelementptr %eframe* %reg_100, i32 0, i32 2, i32 1
	store i64 %reg_106, i64* %reg_107
	%reg_108 = getelementptr %eframe* %reg_100, i32 0, i32 2, i32 1
	%reg_109 = load i64* %reg_108
	ret i64 %reg_109
L_84:
	ret i64 63
}

