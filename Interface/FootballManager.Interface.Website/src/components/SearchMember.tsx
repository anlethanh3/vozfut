import { ChangeEvent, useState } from "react"
import { Button, Accordion, Form, FormGroup, FloatingLabel } from "react-bootstrap";

const SearchMember = (props: { onSearchChanged: (props: { name: string }) => void, onSubmit: (props: { name: string }) => void }) => {
    const { onSearchChanged, onSubmit } = props

    const [name, setName] = useState('')

    const onChanged = (e: any) => {
        const value = e.target.value
        onSearchChanged({ name: name })
        setName(value)
    }

    const onSearch = () => {
        onSubmit({ name: name })
    }

    const onClear = () => {
        onSearchChanged({ name: '' })
        setName('')
    }

    return (
        <Accordion className="my-2" defaultActiveKey="0">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Search box</Accordion.Header>
                <Accordion.Body>
                    <Form>
                        <FormGroup className="mb-3" controlId="addMember.Name">
                            <FloatingLabel controlId="floatingName" label="Name">
                                <Form.Control type="text" placeholder="Input name" value={name} onChange={onChanged} onBlur={onChanged} />
                            </FloatingLabel>
                        </FormGroup>
                    </Form>
                    <Button className="me-2" variant="primary" onClick={onSearch}>
                        Search
                    </Button>
                    <Button variant="secondary" onClick={onClear}>
                        Clear
                    </Button>
                </Accordion.Body>
            </Accordion.Item>
        </Accordion>
    )
}

export default SearchMember