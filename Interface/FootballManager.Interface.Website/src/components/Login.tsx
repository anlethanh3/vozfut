import { useEffect, useState } from 'react';
import Form from 'react-bootstrap/Form'
import FloatingLabel from 'react-bootstrap/FloatingLabel'
import { Modal, Button, Alert, Spinner } from 'react-bootstrap';
import { authenticate, profile } from '../providers/UserApiProvider'
import { AxiosError, HttpStatusCode } from 'axios';
import { ProfileProps } from '../reducers/UserReducer';

export interface LoginRequestProps {
    email: string | undefined,
    password: string | undefined,
    username?: string | undefined,
}

export default function Login(props: { show: boolean, onSubmit: (profile: ProfileProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose } = props
    const [info, setInfo] = useState<LoginRequestProps>({ email: undefined, password: undefined, username: '' })
    const [invalid, setInvalid] = useState({ email: false, password: false })
    const [error, setError] = useState<string | undefined>(undefined)
    const [isSuccess, setIsSuccess] = useState(false)
    const [isLoading, setIsLoading] = useState(false)
    const onValid = () => {
        var check = { email: true, password: true }
        if (info.email) {
            check = { ...check, email: false }
        }
        if (info.password) {
            check = { ...check, password: false }
        }
        setInvalid({ ...check })
        if (!check.email && !check.password) {
            onAuthenticate(info)
        }
    }

    const onClickClose = () => {
        if (!isLoading) {
            onClose()
        }
    }

    const onAuthenticate = (login: LoginRequestProps) => {
        let abort = new AbortController()
        setIsSuccess(false)
        setError(undefined)
        setIsLoading(true)
        authenticate({ signal: abort.signal, data: login })
            .then(response => {
                console.log(response)
                if (response.status !== HttpStatusCode.Ok) {
                    throw new Error("Unauthorized")
                }
                return response.data
            })
            .then(token => profile({ signal: abort.signal, token }))
            .then(response => {
                if (response.status !== HttpStatusCode.Ok) {
                    throw new Error("Unauthorized")
                }
                return response.data
            })
            .then(profile => {
                console.log(profile)
                setIsSuccess(true)
            })
            .catch((ex: Error) => {
                if (ex instanceof AxiosError && ex.response?.status === HttpStatusCode.Unauthorized) {
                    setError("Login Failed!")
                    return
                }
                setError(ex.message)
            })
            .finally(() => {
                setIsLoading(false)
            })
    }

    return (
        <Modal show={show} onHide={onClickClose} backdrop="static" keyboard={false}>
            <Modal.Header closeButton>
                <Modal.Title>Login</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {
                    error &&
                    <Alert show={error !== undefined} variant="danger" onClose={() => setError(undefined)} dismissible>
                        {error}
                    </Alert>
                }
                {
                    isSuccess &&
                    <Alert show={isSuccess} variant="success" onClose={() => setIsSuccess(false)} dismissible>
                        Login successfully!
                    </Alert>
                }
                <Form>
                    <Form.Group className="mb-3" controlId="addMember.Name">
                        <FloatingLabel controlId="floatingName" label="Name">
                            <Form.Control type="text" required isInvalid={invalid.email}
                                placeholder="Input email" value={info.email}
                                disabled={isLoading}
                                onChange={(e) => setInfo({ ...info, email: e.target.value })}
                            />
                            <Form.Control.Feedback type="invalid">Email is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMember.Password">
                        <FloatingLabel controlId="floatingPassword" label="Password">
                            <Form.Control type="password" required isInvalid={invalid.password} placeholder="Input Password" value={info.password}
                                disabled={isLoading}
                                onChange={(e) => setInfo({ ...info, password: e.target.value })} />
                            <Form.Control.Feedback type="invalid">Password is required.</Form.Control.Feedback>
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button disabled={isLoading} variant="primary" onClick={onValid}>
                    {
                        isLoading && <Spinner animation="border" variant="light" />
                    }
                    {
                        !isLoading && `Login`
                    }

                </Button>
            </Modal.Footer>
        </Modal>
    )
}