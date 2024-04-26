import { SafeHtml } from "@angular/platform-browser";

export interface ArticleDto{
    title: string,
    author: string,
    creationDate: Date,
    content: SafeHtml,
    tags: string[]
}