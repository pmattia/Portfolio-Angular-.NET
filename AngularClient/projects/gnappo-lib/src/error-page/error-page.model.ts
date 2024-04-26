export interface ErrorPageModel {
    title: string;
    text: string;
    actions: { label: string, action: () => void }[];
}