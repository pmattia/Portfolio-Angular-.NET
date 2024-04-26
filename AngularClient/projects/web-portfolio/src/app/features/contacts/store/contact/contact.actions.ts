import { createAction, props } from "@ngrx/store";
import { ContactMessageDto } from "../../../../core/dto/request/contact-message.dto";

export const sendContactMessage = createAction('[CONTACT] Send message ',
    props<{ contactMessage: ContactMessageDto }>()
);
export const sendContactMessageSuccess = createAction('[CONTACT] Send message success',
    props<{ contactMessage: ContactMessageDto }>()
);
export const sendContactMessageFailure = createAction('[CONTACT] Send message failure',
    props<{ error: string }>()
);