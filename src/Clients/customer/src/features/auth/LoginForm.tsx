export function LoginForm() {
  return (
    <form className="space-y-4">
      <div>
        <label className="block text-md">Username</label>
        <input
          type="text"
          className="mt-1 block w-full border border-gray-300  p-2"
          placeholder="Enter your username"
        />
      </div>
      <div>
        <label className="block text-md ">Password</label>
        <input
          type="password"
          className="mt-1 block w-full border border-gray-300  p-2"
          placeholder="Enter your password"
        />
      </div>
      <div className="text-sm text-gray-500">Tips: For testing, you can use user/User@123.</div>
      <button
        type="submit"
        className="w-full bg-blue-500 text-white py-2 px-4"
      >
        Sign in
      </button>
    </form>
  );
}
