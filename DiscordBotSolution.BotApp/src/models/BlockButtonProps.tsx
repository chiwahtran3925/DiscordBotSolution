export interface BlockButtonProps {
    username: string;
    isBlocked: boolean;
    onBlock: (isBlocked: boolean) => void;
}