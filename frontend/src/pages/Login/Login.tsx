import { WrapperForm, LoginForm } from "./components";

export const Login = () => {
  return (
    <div className='w-screen h-screen bg-gray-100 flex justify-center items-center'>
      <WrapperForm>
        <LoginForm />
      </WrapperForm>
    </div>
  );
};
