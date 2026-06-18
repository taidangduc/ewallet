export const TextField = ({
  label,
  name,
  type = "text",
  placeholder,
  errorMessage,
  ...props
}: React.InputHTMLAttributes<HTMLInputElement> & {
  label: string;
  name: string;
  type?: "text" | "email" | "password" | "number";
  placeholder?: string;
  errorMessage?: string;
}) => {
  return (
    <div className="relative">
      <label className="block text-md">{label}</label>
      <input
        name={name}
        type={type}
        className={`
              mt-1 block w-full outline px-3 py-2 outline 
              focus:rounded-[2px]
              focus:outline-none 
              ${
                errorMessage
                  ? "outline-red-500 focus:ring-2 focus:ring-red-500"
                  : "outline-gray-300 focus:ring-2 focus:ring-black"
              }             
            `}
        placeholder={placeholder}
        {...props}
      />
      <div className="absolute -bottom-5 left-0 flex items-center space-x-1">
        {errorMessage && (
          <span className=" text-red-500 font-medium text-[12px] mt-1">
            {errorMessage}
          </span>
        )}
      </div>
    </div>
  );
};
