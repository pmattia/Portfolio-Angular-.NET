import { ContactMessageDto } from "../../../core/dto/request/contact-message.dto";

export interface ContactState{
    isLoading: boolean;
    contactMessage?: ContactMessageDto
}
