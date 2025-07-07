type Props = {
  title: string;
};

export const BlogPost = ({ title }: Props) => {
  return (
    <div>
      <p>{title}</p>
    </div>
  );
};
