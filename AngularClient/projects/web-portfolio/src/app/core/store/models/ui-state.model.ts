import { TopicInfoDto } from "../../dto/response/article-info.dto";

export interface UiState{
    mainTopics: TopicInfoDto[],
    asideShown: boolean
    greetingShown: boolean
    isLoading: boolean;
    error?: string;
    isMobile: boolean;
}