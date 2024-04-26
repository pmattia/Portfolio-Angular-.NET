import { SafeHtml } from "@angular/platform-browser";

export interface BlogPostDto{
    title: string,
    author: string,
    creationDate: Date,
    content: SafeHtml,
    tags: string[]
}